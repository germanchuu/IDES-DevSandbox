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

public interface IDataDeletable<T>
{
    void DeleteData(T data);
}

public interface IDataUpdatable<T>
{
    void UpdateData(T data);
}

public interface IDataHandler<TInsert, TGet, TDelete, TUpdate> 
    : IDataInsertable<TInsert>, IDataGetable<TGet>, IDataDeletable<TDelete>, IDataUpdatable<TUpdate>
{
    
}

