
namespace Modul_1_Base_Class_Library.Models
{
    class Rule
    {
        public string FilePattern { get; set; }
        public string DestinationFolder { get; set; }
        public bool IsOrderAppended { get; set; }
        public bool IsDateAppended { get; set; }

        public int MatchesCount { get; set; }
    }
}
