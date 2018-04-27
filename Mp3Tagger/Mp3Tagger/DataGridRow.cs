using System.Collections.Generic;

namespace Mp3Tagger
{
    public class DataGridRow
    {
        public string Column1 { get; set; }
        //+ the other properties...
    }

    public class DataGridRows : List<DataGridRow> //or use your real data item class
    {
    }
}