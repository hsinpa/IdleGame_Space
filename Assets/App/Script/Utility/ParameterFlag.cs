
public class ParameterFlag
{
    public class LayerIndex
    {
        public const int Normal = 0;
        public const int IgnoreRaycast = 2;
    }

    public class CSVFileName {
        public const string CharateristicsList = "database - characteristic.csv";
        public const string FirstNameList = "database - character first name.csv";
        public const string SurnameList = "database - character family name.csv";
        public const string Task = "database - task.csv";

    }

    public class SaveSlotKey {
        public const string Character = "hired_character@save";


    }


    public class CharacterJSONKey
    {
        public const string Character = "lastname";
        //public const string Character = "firstname";
        //public const string Character = "_id";
        //public const string Character = "negative_id";
        //public const string Character = "positive_id";

    }


}

public enum ModalType
{
    CharacterModal
}
