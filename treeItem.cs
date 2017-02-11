using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aga.Controls.Tree;
using System.Drawing;

namespace DBManager
{
    public class treeItem : Node
    {
        public string nodeText   {get; set;}
        public string fieldType { get; set; }
        public string fieldSize { get; set; }
        public bool allowNull { get; set; }
        public ImageList imageList { get; set; }
        internal int _imageIndex;  //{ get; set; }
        public Bitmap nodeIcon { get; set; }
        public bool nodeCheck { get; set; }
        public string fieldTypeInternal { get; set; }

        public treeItem(string nodeText, string fieldType, string fieldSize, bool allowNull, bool nodeCheck, string fieldTypeInternal,int imgIndex, ImageList imgList)
        {
            this.nodeText = nodeText;
            this.fieldType = fieldType;
            this.fieldSize = fieldSize;
            this.allowNull = allowNull;
            this.nodeCheck = nodeCheck;
            this.fieldTypeInternal = fieldTypeInternal;

            this.imageList = imgList;
            this.imageIndex = imgIndex;
        }

        public int imageIndex
        {
            get
            {
                return _imageIndex;
            }
            set
            {
                _imageIndex = value;
                nodeIcon = new Bitmap(imageList.Images[value]);
            }
        }
    }
}
