using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace StopWatch
{
    class StopWatchViewModel : BaseViewModel
    {

        //PUSHTEST
        DispatcherTimer dispatcherTimer;
        DateTime StartTime;
        TimeSpan starttime;

        /// <summary>
        /// 経過時間表示
        /// </summary>
        private string dispTime;

        /// <summary>
        /// 経過時間表示
        /// </summary>
        public string DispTime
        {
            set
            {
                dispTime = value;
                OnPropertyChanged();
            }
            get
            {
                return dispTime;
            }
        }

        private bool stopFlag;

        /// 起動コマンド
        /// </summary>
        private ICommand start;

        /// <summary>
        /// 起動コマンド
        /// </summary>
        public ICommand Start
        {
            get
            {
                if (null == start)
                {
                    start = new DelegateCommand(StartWatch);

                }
                return start;
            }
        }

        /// 停止コマンド
        /// </summary>
        private ICommand stop;

        /// <summary>
        /// 停止コマンド
        /// </summary>
        public ICommand Stop
        {
            get
            {
                if (null == stop)
                {
                    stop = new DelegateCommand(StopWatch);

                }
                return stop;
            }
        }

        /// リセットコマンド
        /// </summary>
        private ICommand reset;

        /// <summary>
        /// リセットコマンド
        /// </summary>
        public ICommand Reset
        {
            get
            {
                if (null == reset)
                {
                    reset = new DelegateCommand(ResetWatch);

                }
                return reset;
            }
        }

        /// <summary>
        /// タイマー起動
        /// </summary>
        private void StartWatch()
        {
            if (stopFlag == true)
            {
                StartTime = DateTime.Now;
                stopFlag = false;
            }
            dispatcherTimer.Start();
        }

        /// <summary>
        /// タイマー停止
        /// </summary>
        private void StopWatch()
        {
            if (stopFlag == false)
            {
                dispatcherTimer.Stop();
                stopFlag = true;
                //StartTime = DateTime.MinValue;
            }
        }

        /// <summary>
        /// タイマーリセット
        /// </summary>
        private void ResetWatch()
        {
            if (stopFlag == true)
            {
                StartTime = DateTime.MinValue;
            }
            DispTime = "00:00:000";
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public StopWatchViewModel()
        {
            StartTime = DateTime.MinValue;
            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            stopFlag = true;
        }

        /// <summary>
        /// タイマーのインターバルで発生する画面表示メソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // 起動時間から現在までの経過時間を算出
            starttime = DateTime.Now.Subtract(StartTime);

            // 表示されている経過時間に新たに経過時間を追加する。
            //DispTime = sumtime.Add(starttime).ToString(@"mm\:ss\:fff");
            DispTime = starttime.ToString(@"mm\:ss\:fff");
        }
    }
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// 現在の状態でこのコマンドを実行できるかどうかを判断するメソッドを定義します。
        /// </summary>
        private Func<bool> canExecute;

        /// <summary>
        /// 現在の状態でこのコマンドを実行できるかどうかを判断するメソッドを定義します。
        /// </summary>
        private Action execute;

        /// <summary>
        /// コマンドの起動時に実行するメソッドを指定してインスタンスを生成、初期化します。
        /// </summary>
        /// <param name="execute">コマンドの起動時に実行するメソッド</param>
        public DelegateCommand(Action execute)
        {
            this.execute = execute;
            this.canExecute = null;
        }

        /// <summary>
        /// コマンドの起動時に実行するメソッドとコマンドを実行するかどうかを返すメソッドを指定してインスタンスを生成、初期化します。
        /// </summary>
        /// <param name="execute">コマンドの起動時に実行するメソッド</param>
        /// <param name="canExecute">コマンドを実行するかどうかを返すメソッド</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// コマンドを実行するかどうかに影響するような変更があった場合に発生します。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 現在の状態でこのコマンドを実行できるかどうかを返します。
        /// </summary>
        /// <param name="parameter">コマンドで使用されたデータ。 コマンドにデータを渡す必要がない場合は、このオブジェクトを null に設定されます。 </param>
        /// <returns>このコマンドを実行できる場合は true。それ以外の場合は false。</returns>
        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            return this.canExecute();
        }

        /// <summary>
        /// コマンドの起動時に呼び出されます。
        /// </summary>
        /// <param name="parameter">コマンドで使用されたデータ。 コマンドにデータを渡す必要がない場合は、このオブジェクトを null に設定されます。</param>
        public void Execute(object parameter)
        {
            this.execute();
        }
    }
}

    