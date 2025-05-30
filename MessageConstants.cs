using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qa_dotnet_cucumber
{
    public static class MessageConstants
    {
        public const string SuccessMessageForAdd = "Education has been added";
        public const string SuccessMessageForUpdate = "Education has been updated";
        public const string ErrorMessage = "Education information was invalid";
        public const string ErrorMessageForEmptyField = "Please enter all the fields";
        public const string ErrorMessageForAddingDuplicateDetails = "This information is already exist.";
        public const string ErrorMessageForSessionExpired = "undefined";
    }

}

