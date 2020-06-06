using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;
using SQLHelperLibrary;

namespace MisakaTranslator_WPF
{
    /// <summary>
    /// GameNameDialog.xaml 的交互逻辑
    /// </summary>
    public partial class GameNameDialog : UserControl
    {
        List<GameInfo> gameInfolst;
        int gid; //当前选中的顺序，并非游戏ID
        public GameNameDialog(List<GameInfo> gameInfo, int id)
        {
            InitializeComponent();
            gameInfolst = gameInfo;
            gid = id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (nameBox.Text != "")
            {
                GameLibraryHelper.UpdateGameNameByID(gameInfolst[gid].GameID, nameBox.Text);
                HandyControl.Controls.MessageBox.Show("已修改，重启后生效！", "提示");
            }

        }
    }
}