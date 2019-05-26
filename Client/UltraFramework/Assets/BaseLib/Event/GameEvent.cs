﻿//-----------------------------------------------------------------------
//| by:Qcbf                                                             |
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;


/// <summary>
/// event execute
/// </summary>
public struct GameEventInfo
{
    public GameEventHandler eventHandler;
    public object[] args;
}


/// <summary>
/// Event handler.
/// external event function format
/// </summary>
public delegate void GameEventHandler(params object[] args);


public class GameEvent : IGameEvent
{
    /// <summary>
    /// The same event id max number.
    /// </summary>
    public static ushort SameSyncEventMax = 10;


    /// <summary>
    /// event execute list
    /// </summary>
    private List<GameEventInfo> mAsyncEventList;


    /// <summary>
    /// The event list.
    /// </summary>
    protected Dictionary<Enum, List<GameEventHandler>> mEventDic;

    /// <summary>
    /// just use once of event
    /// </summary>
    protected Dictionary<Enum, List<GameEventHandler>> mUseOnceEventDic;

    /// <summary>
    /// 
    /// </summary>
    public virtual bool IsDispose
    {
        get;
        set;
    }


    public GameEvent()
    {
        IsDispose = false;
        mAsyncEventList = new List<GameEventInfo>();
        mEventDic = new Dictionary<Enum, List<GameEventHandler>>();
        mUseOnceEventDic = new Dictionary<Enum, List<GameEventHandler>>();
    }


    /// <summary>
    /// Adds the event.
    /// </summary>
    public virtual void AddEvent(Enum type, GameEventHandler handler, bool isUseOnce = false, bool isFirst = false)
    {
        List<GameEventHandler> handlerList = mEventDic.ContainsKey(type) ? mEventDic[type] : null;
        if (handlerList == null)
        {
            mEventDic[type] = new List<GameEventHandler>();
        }
        if (mEventDic[type].Contains(handler))
            return;

        if (isFirst)
            mEventDic[type].Insert(0, handler);
        else
            mEventDic[type].Add(handler);

        if (isUseOnce)
        {
            if (!mUseOnceEventDic.ContainsKey(type))
                mUseOnceEventDic.Add(type, new List<GameEventHandler>());
            mUseOnceEventDic[type].Add(handler);
        }
    }



    /// <summary>
    /// Removes the event.
    /// </summary>
    public virtual void RemoveEvent(Enum type, GameEventHandler handler)
    {
        List<GameEventHandler> handlerList = mEventDic.ContainsKey(type) ? mEventDic[type] : null;
        if (handlerList != null && handlerList.Contains(handler))
            handlerList.Remove(handler);

        if (mUseOnceEventDic.ContainsKey(type) && mUseOnceEventDic[type].Contains(handler))
            mUseOnceEventDic[type].Remove(handler);
    }


    /// <summary>
    /// Removes All this type of event.
    /// </summary>
    public virtual void RemoveEvent(Enum type)
    {
        if (mEventDic.ContainsKey(type))
            mEventDic.Remove(type);
        if (mUseOnceEventDic.ContainsKey(type))
            mUseOnceEventDic.Remove(type);
    }


    /// <summary>
    /// remove all event
    /// </summary>
    public virtual void RemoveEvent()
    {
        mEventDic.Clear();
        mUseOnceEventDic.Clear();
    }


    /// <summary>
    /// Dispatch the specified type, target and args. sync type. 
    /// </summary>
    public virtual void DispatchEvent(Enum type, params object[] args)
    {
        List<GameEventHandler> handlerList = mEventDic.ContainsKey(type) ? mEventDic[type] : null;
        if (handlerList != null && HasEvent(type))
        {
            for (int i = handlerList.Count - 1; i >=0; i--) {
                GameEventHandler handler = handlerList[i];
                if (mUseOnceEventDic.ContainsKey(type) && mUseOnceEventDic[type].Contains(handlerList[i])) {
                    RemoveEvent(type, handler);
                    //i--;
                }
                try {
                    handler(args);
                } catch (Exception e) {
                    Log.Warning("DispatchEvent error", e.Message, e.StackTrace);
                }
                if (args.Length > 0 && args[0] is IGameEventArgs && (args[0] as IGameEventArgs).IsCancelDefaultAction)
                    break;
            }
        }
    }

    /// <summary>
    /// Dispatch the specified type, target and args. async type, in idle frame execute function
    /// </summary>
    public virtual void DispatchAsyncEvent(Enum type, params object[] args)
    {
        List<GameEventHandler> handlerList = mEventDic.ContainsKey(type) ? mEventDic[type] : null;
        if (handlerList != null && HasEvent(type))
        {
            for (short i = 0; i < handlerList.Count; i++)
            {
                mAsyncEventList.Add(new GameEventInfo()
                        {
                            args = args,
                            eventHandler = handlerList[i]
                        });
                if (mUseOnceEventDic.ContainsKey(type) && mUseOnceEventDic[type].Contains(handlerList[i]))
                {
                    RemoveEvent(type, handlerList[i]);
                    i--;
                }
            }
        }
    }

    /// <summary>
    /// Dispatch the specified type, target and args. in new child thread execute function
    /// </summary>
    [System.Obsolete("Do not use temporary")]
    public virtual void DispatchThreadEvent(Enum type, object args)
    {
        ///////////////
        List<GameEventHandler> handlerList = mEventDic.ContainsKey(type) ? mEventDic[type] : null;
        if (handlerList != null && HasEvent(type))
        {
            for (short i = 0; i < handlerList.Count; i++)
            {
                Thread thread = new Thread((object arg) => handlerList[i]());
                thread.Start(args);
                if (mUseOnceEventDic.ContainsKey(type) && mUseOnceEventDic[type].Contains(handlerList[i]))
                {
                    RemoveEvent(type, handlerList[i]);
                    i--;
                }
            }
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual bool HasEvent(Enum type)
    {
        return mEventDic.ContainsKey(type);
    }


    /// <summary>
    /// 消息循环
    /// </summary>
    public virtual void UpdateEvent()
    {
        for (short i = 0; i < mAsyncEventList.Count; i++)
        {
            GameEventInfo taskEvent = mAsyncEventList[i];
            mAsyncEventList.Remove(taskEvent);
            try
            {
                taskEvent.eventHandler(taskEvent.args);
            }
            catch (Exception e)
            {
                Log.Warning("UpdateEvent error", e.Message, e.StackTrace);
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public virtual void Dispose()
    {
        IsDispose = true;
        mUseOnceEventDic.Clear();
        mEventDic.Clear();
        mAsyncEventList.Clear();
        mAsyncEventList = null;
        mEventDic = null;
        mUseOnceEventDic = null;
    }



}










