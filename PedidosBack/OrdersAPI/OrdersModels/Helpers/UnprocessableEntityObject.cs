using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersModels.Helpers
{
    public class UnprocessableEntityObject : ObjectResult
    {
        public UnprocessableEntityObject(ModelStateDictionary value) : base(value)
        {
        }
    }
}
