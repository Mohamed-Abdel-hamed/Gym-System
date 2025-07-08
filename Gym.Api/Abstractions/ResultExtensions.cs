using Microsoft.AspNetCore.Mvc;

namespace Gym.Api.Abstractions;

public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result,int statusCode,string title)
    {
       if(result.IsSuccess)
            throw new InvalidOperationException("cannot convert to Problem");



        ProblemDetails problemDetails = new()
        {
            Status = statusCode,
            Title = title,
            Extensions = new Dictionary<string, object?>
            {
                {
                    "errors",new [] { result.Error}
                }
            }
        };

        return new ObjectResult(problemDetails);

    }
}
