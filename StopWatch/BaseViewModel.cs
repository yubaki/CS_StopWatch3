using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StopWatch
{
     public class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        // INotifyPropertyChangedインタフェースを実装する
        // event構文を用いることで、Add,Deleteを省略できる。
        // PropertyのDelegate版

        /// <summary>
        /// プロパティ値が変更されたことをクライアントに通知します。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        // 例１ 

        /// <summary>
        /// <para>PropertyChanged イベント を発生させます。</para>
        /// <para>CallerMemberNameからプロパティ名を取得。</para>
        /// </summary>
        /// <param name="propertyName">変更されたプロパティの名前</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// PropertyChanged イベント を発生させます。
        /// </summary>
        /// <param name="propertyName">変更されたプロパティの名前</param>
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    if (this.PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        // 例２
        
        /// <summary>
        /// <para>PropertyChanged イベント を発生させます。</para>
        /// <para>Expression<Func<T>>からプロパティを取得し、プロパティ名を取得</para>
        /// </summary>
        /// <param name="propertyName">変更されたプロパティ</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyExpression.Name));
            }
        }

        // IDataErrorInfoインタフェースを実装する
        //0902

        //IDataErrorInfo用のエラーメッセージを保持する辞書
        private Dictionary<string, string> _ErrorMessages = new Dictionary<string, string>();

        //IDataErrorInfo.Errorの実装
        string IDataErrorInfo.Error
        {
            get { return (_ErrorMessages.Count > 0) ? "Has Error" : null; }
        }

        //IDataErrorInfo.Itemの実装    実際にエラーメッセージを取り出す際に必要な記述
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (_ErrorMessages.ContainsKey(columnName))
                    return _ErrorMessages[columnName];
                else
                    return null;
            }
        }

        //エラーメッセージのセット
        protected void SetError(string propertyName, string errorMessage)
        {
            _ErrorMessages[propertyName] = errorMessage;
        }

        //エラーメッセージのクリア
        protected void ClearError(string propertyName)
        {
            if (_ErrorMessages.ContainsKey(propertyName))
                _ErrorMessages.Remove(propertyName);
        }

        //エラーメッセージが入っているかどうか
        /// <summary>
        /// エラーメッセージが入っているかどうか
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>登録ずみならtrue</returns>
        protected bool CheckError(string propertyName)
        {
            bool ret = _ErrorMessages.ContainsKey(propertyName);
            return ret;
        }
    }
}
