using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.Infrastructure.Images
{
    public class CutAvatarParams
    {
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
        /// <summary>
        /// 源图展现尺寸宽度（客户端展现源图时，可能有缩放）
        /// </summary>
        public int srcClientWidth { get; set; }
        public int srcClientHeight { get; set; }
        /// <summary>
        /// 源图相对文件路径（虚拟）
        /// </summary>
        public string imgSrcFilePath { get; set; }
        /// <summary>
        /// 源图文件实际路径
        /// </summary>
        public string imgSrcFileRealPath { get; set; }

        /// <summary>
        /// 头像相对目录路径（虚拟）
        /// </summary>
        public string imgAvatarFilePath { get; set; }
        /// <summary>
        /// 头像目录实际路径
        /// </summary>
        public string imgAvatarRealPath { get; set; }
    }
}
