using System.Collections;
using System.Collections.Generic;

public interface IDataInsertable<T>
{   
    void InsertData(T data);
}

public interface IDataGetable<T>
{
    T GetData();
}

public interface IDataHandler<TInsert, TGet> : IDataInsertable<TInsert>, IDataGetable<TGet>
{
    
}
