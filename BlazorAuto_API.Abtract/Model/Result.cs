using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Abstract;

public class Result
{
    public bool Success { get; set; } = false;

    public string Message { get; set; } = "";

    public Result()
    {
    }
    public static Result Ok(string mess)
    {
        return new Result { Success = true, Message = mess };
    }
    public static Result Ok()
    {
        return new Result { Success = true };
    }

    public static Result Error(string error)
    {
        return new Result { Message = error };
    }

    public static implicit operator Result(string error)
    {
        return Result.Error(error);
    }

    public static implicit operator Result(bool success)
    {
        return new Result { Success = success };
    }

    public static implicit operator Result((bool, string) result)
    {
        return result;
    }
}

public class ResultOf<T> : Result
{
    public ResultOf() { 
    }

    public ResultOf(bool s, string m, T i)
    {
        Success = s;
        Message = m;
        Item = i;
    }


    public T Item { get; set; } = default!;


    public static ResultOf<T> Ok(T item)
    {
        return new ResultOf<T> { Success = true, Item = item };
    }

    public static ResultOf<T> Ok(T data, string msg)
    {
        return new ResultOf<T> { Success = true, Item = data, Message = msg };
    }

    public static new ResultOf<T> Error(string msg = "")
    {
        return new ResultOf<T> { Message = msg };
    }


    public static implicit operator ResultOf<T>(string error)
    {
        return Error(error);
    }

    public static implicit operator ResultOf<T>(T item)
    {
        if (item == null)
        {
            return "Not found: Dev not check null";
        }
        return ResultOf<T>.Ok(item);
    }

    public static implicit operator ResultOf<T>((bool, string, T) result)
    {
        return new ResultOf<T>(result.Item1, result.Item2, result.Item3);
    }

    public static implicit operator ResultOf<T>((T, string) result)
    {
        return new ResultOf<T>(true, result.Item2, result.Item1);
    }
}

public class ResultsOf<T> : Result
{

    public IEnumerable<T> Items { get; set; } = [];

    public ResultsOf(bool success, string message, IEnumerable<T> items)
    {
        Success = success;
        Message = message;
        Items = items;
    }

    public ResultsOf()
    {
    }

    public static ResultsOf<T> Ok(IEnumerable<T> items)
    {
        return new ResultsOf<T> { Success = true, Items = items };
    }

    public static new ResultsOf<T> Error(string msg = "")
    {
        return new ResultsOf<T> { Message = msg };
    }

    public static implicit operator ResultsOf<T>(string error)
    {
        return ResultsOf<T>.Error(error);
    }

    public static implicit operator ResultsOf<T>(T[] items)
    {
        return ResultsOf<T>.Ok(items);
    }

    public static implicit operator ResultsOf<T>(List<T> items)
    {
        return ResultsOf<T>.Ok(items);
    }

    public static implicit operator T[](ResultsOf<T> result)
    {
        return result.Success ? result.Items.ToArray() : [];
    }
    public static implicit operator ResultsOf<T>((bool, string, IEnumerable<T> T) result)
    {
        return new ResultsOf<T>(result.Item1, result.Item2, result.Item3);
    }

    public static implicit operator ResultsOf<T>((IEnumerable<T>, string) result)
    {
        return new ResultsOf<T>(true, result.Item2, result.Item1);
    }
}

public class PagedResultsOf<T> : Result
{

    public IEnumerable<T> Items { get; set; }=[];

    public int TotalCount { get; set; }


    public static PagedResultsOf<T> Empty()
    {
        return new PagedResultsOf<T> { Success = true, Items = new List<T>(), TotalCount = 0 };
    }

    public static PagedResultsOf<T> Ok(IEnumerable<T> data, int totalCount)
    {
        return new PagedResultsOf<T> { Success = true, Items = data, TotalCount = totalCount };
    }

    public static new PagedResultsOf<T> Error(string msg = "")
    {
        return new PagedResultsOf<T> { Message = msg };
    }

    public static implicit operator PagedResultsOf<T>(string error)
    {
        return PagedResultsOf<T>.Error(error);
    }

    public static implicit operator PagedResultsOf<T>(List<T> data)
    {
        return new PagedResultsOf<T> { Success = true, Items = data, TotalCount = data.Count };
    }
}
