using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionSystem : Singleton<ActionSystem>
{
    private List<GameAction> reactions = null;
    private static Dictionary<(Delegate, Type, ReactionTiming), Action<GameAction>> wrappedDelegates = new();
    public bool IsPerforming { get; private set; } = false;
    private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();
    private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();
    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        ClearStatics();
    }

    private void OnDestroy()
    {
        ClearStatics();
    }

    private void ClearStatics()
    {
        wrappedDelegates.Clear();
        preSubs.Clear();
        postSubs.Clear();
        performers.Clear();
    }
    public void Perform(GameAction action, System.Action OnPerformFinished = null)
    {
        if (IsPerforming)
        {
            return;
        }
        IsPerforming = true;
        StartCoroutine(RootFlow(action, OnPerformFinished));
    }
    private IEnumerator RootFlow(GameAction action, Action onFinished)
    {
        try
        {
            yield return Flow(action);
        }
        finally
        {
            IsPerforming = false;
            onFinished?.Invoke();
        }
    }
    public void AddReaction(GameAction gameAction)
    {
        if (reactions == null) 
        {
            Debug.LogWarning("AddReaction called outside of a Flow — ignored.");
            return;
        }
        reactions.Add(gameAction);
    }

    private IEnumerator Flow(GameAction action)
    {
        reactions = action.PreReactions;
        PerformSubscribers(action, preSubs);
        yield return PerformReactions();

        reactions = action.PerformReactions;
        yield return PerformPerformer(action);
        yield return PerformReactions();

        reactions = action.PostReactions;
        PerformSubscribers(action, postSubs);
        yield return PerformReactions();
    }
    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();
        if (performers.ContainsKey(type))
        {
            yield return performers[type](action);
        }
    }
    private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type type = action.GetType();
        if (!subs.TryGetValue(type, out var list)) return;

        foreach (var sub in list.ToList())
        {
            sub(action);
        }
        
    }
    private IEnumerator PerformReactions()
    {
        foreach (var reaction in reactions)
        {
            yield return Flow(reaction);
        }
    }

    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);
        if (performers.ContainsKey(type))
        {
            performers[type] = wrappedPerformer;
        }
        else
        {
            performers.Add(type, wrappedPerformer);
        }
    }
    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (performers.ContainsKey(type))
        {
            performers.Remove(type);
        }
    }
    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        var key = (reaction, typeof(T), timing);
        if (wrappedDelegates.ContainsKey(key)) return;

        Action<GameAction> wrappedReaction = action => reaction((T)action);
        wrappedDelegates[key] = wrappedReaction;

        var subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        if (!subs.TryGetValue(typeof(T), out var list))
        {
            list = new List<Action<GameAction>>();
            subs[typeof(T)] = list;
        }

        list.Add(wrappedReaction);
    }
    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        var key = (reaction, typeof(T), timing);
        if (!wrappedDelegates.TryGetValue(key, out var wrappedReaction)) return;

        var subs = timing == ReactionTiming.PRE ? preSubs : postSubs;

        if (subs.TryGetValue(typeof(T), out var list))
        {
            list.Remove(wrappedReaction);
        }

        wrappedDelegates.Remove(key);
    }
}

