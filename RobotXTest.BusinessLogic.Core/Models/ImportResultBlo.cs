namespace RobotXTest.BusinessLogic.Core.Models
{
    public class ImportResultBlo
    {
        public int Added { get; set; }
        public int Updated { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
