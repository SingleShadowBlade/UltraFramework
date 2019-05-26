//-----------------------------------------------------------------------
//| by:Qcbf                                                             |
//-----------------------------------------------------------------------

using System;
public interface IGameEvent
{
	void AddEvent ( Enum type, GameEventHandler handler, bool isUseOnce = false, bool isFirst = false );
	void RemoveEvent ( Enum type, GameEventHandler handler );
	void RemoveEvent ( Enum type );
	void RemoveEvent ();
	void DispatchEvent ( Enum type, params object[] args );
	void DispatchAsyncEvent ( Enum type, params object[] args );
	bool HasEvent ( Enum type );
	void Dispose ();
	bool IsDispose
	{
		get;
	}

}