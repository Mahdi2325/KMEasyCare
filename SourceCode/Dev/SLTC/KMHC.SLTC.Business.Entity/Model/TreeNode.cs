namespace KMHC.SLTC.Business.Entity.Model
{
    using System;
    using System.Collections.Generic;

    public partial class TreeNode
    {
        public string moduleId { get; set; }
        public string text { get; set; }
        public string href { get; set; }
        public State state { get; set; }
        public List<TreeNode> nodes { get; set; }

        public TreeNode()
        {
            //this.nodes = new List<TreeNode>();
        }
    }

    public partial class State
    {
        public bool @checked { get; set; }
        //public bool disabled { get; set; }
        //public bool expanded { get; set; }
        //public bool selected { get; set; }
    } 
}
