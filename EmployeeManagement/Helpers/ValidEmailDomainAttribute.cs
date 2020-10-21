using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Helpers
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        public string _validDomain { get; }

        public ValidEmailDomainAttribute(string validDomain)
        {
            _validDomain = validDomain;
        }

        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');
            return strings[1].ToLower() == _validDomain.ToLower();
        }
    }
}
