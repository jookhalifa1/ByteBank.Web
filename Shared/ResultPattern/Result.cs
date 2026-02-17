using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ResultPattern
{
     public class Result
    {
        public bool IsSuccess => Errors.Count == 0;
        public bool IsFailure => !IsFailure;
        public List<Error> Errors { get; } = [];



        // OK()

        protected Result() { }

        // Failure
        protected Result (Error error)
        {
            Errors.Add(error);
        }

        protected Result( IReadOnlyList<Error> error)
        {
            Errors.AddRange(error);
        }


        public static Result Ok() => new Result();
        public static Result Failure(Error error) => new Result(error);
        public static Result Failure(IReadOnlyList<Error> errors) => new Result(errors);
    }


    public class Result<TValue> : Result
    {
         private TValue tValue;

        public TValue Value => IsSuccess ? tValue : throw new InvalidOperationException(" Can Not Access this value");


        private Result(TValue value ):base()
        {
            tValue = value;
        }

        private Result(Error error) : base(error)
        {
            tValue = default;
        }
        private Result(IReadOnlyList<Error> errors) : base(errors)
        {
            tValue = default;
        }

        public static Result<TValue>Ok( TValue value)=>new Result<TValue>(value);   
        public static Result<TValue>Failure( Error value)=>new Result<TValue>(value);   
        public static Result<TValue>Failure( IReadOnlyList<Error> value)=>new Result<TValue>(value);


        public static implicit operator Result<TValue>(TValue value) => Ok(value);

        public static implicit operator Result<TValue>(Error error) => Failure(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Failure(errors);




    }
}
