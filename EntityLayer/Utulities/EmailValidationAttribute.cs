
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EntityLayer.Utulities
{
    public class EmailValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value==null)
            {
                return false;
            }
            if (value.ToString().Contains("@") && Regex.Matches(value.ToString(),"@").Count()==1)
            {
                var result = value.ToString().Split("@");
                if (result[1].ToLower() == "gmail.com")
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
