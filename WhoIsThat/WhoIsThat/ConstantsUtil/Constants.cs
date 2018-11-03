using System;
using System.Collections.Generic;
using System.Text;

namespace WhoIsThat.ConstantsUtil
{
    public static class Constants
    {
        //Recognition
        public const string NoFacesIdentifiedError = "No faces were detected!";
        
        public const string NoMatchFoundError = "No one was indetified!";
        
        public const string WrongUriError = "Photo was not successfully taken!";

        public const string PersonNotCreatedError = "Person was not successfully registered!";
        
        //Storage
        public const string TargetNotFoundError = "This is not your target";

        public const string UserDoesNotExistError = "User with ID was not found";

        public const string InvalidImageUriAndNameError = "Something went wrong with uploading photo, please try again";

        public const string TargetAlreadyAssignedError = "You already have target assigned";

        public const string TargetNotAssignedError = "Target was not assigned";

        public const string ThereAreNoPlayersError = "There are no other players";

        public const string FatalStorageError = "Something really bad happened, please try again";
    }
}
