using GalaSoft.MvvmLight;
using MaterialDesignThemes.Wpf;
using Olwen_2._0._0.Model;
using Olwen_2._0._0.Repositories.Implements;
using Olwen_2._0._0.Repositories.Interfaces;
using Olwen_2._0._0.View.Components;
using Olwen_2._0._0.View.DialogsResult;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Olwen_2._0._0.ViewModel
{
    public class LogicstisViewModel:ViewModelBase
    {
        private IAsyncRepository<Logistic> log_repo;
        private const string DialogHostId = "hostid";
        private ObservableCollection<Logistic> _listLog;
        private string _logID, _name, _fee;
        static DialogContent dc = new DialogContent();
        static DialogOk dialog = new DialogOk();

        public DelegateCommand<int?> UpdateCommand
        {
            get;
            private set;
        }

        public DelegateCommand<string> SubmitCommand
        {
            get;
            private set;
        }

        public DelegateCommand<int?> DeleteCommand
        {
            get;private set;
        }


        public DelegateCommand AddCommand
        {
            get;
            private set;
        }

        public DelegateCommand LoadedCommand
        {
            get;
            private set;
        }

        public ObservableCollection<Logistic> ListLog
        {
            get
            {
                return _listLog;
            }

            set
            {
                _listLog = value;
                RaisePropertyChanged();
            }
        }

        public string LogID
        {
            get
            {
                return _logID;
            }

            set
            {
                _logID = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public string Fee
        {
            get
            {
                return _fee;
            }

            set
            {
                _fee = value;
                RaisePropertyChanged();
            }
        }

        public LogicstisViewModel()
        {
            log_repo = new BaseAsyncRepository<Logistic>();
            LoadedCommand = new DelegateCommand(CreateData);
            SubmitCommand = new DelegateCommand<string>(Submit);
            AddCommand = new DelegateCommand(CreateNewData);
            DeleteCommand = new DelegateCommand<int?>(Delete);
        }

        private async void Delete(int? obj)
        {
            try
            {
                dc = new DialogContent() { Content = "Bạn muốn xóa vận chuyển này ?", Tilte = "Thông Báo" };
                var dialogYS = new DialogYesNo() { DataContext = dc };
                var result = (bool)await DialogHost.Show(dialogYS, DialogHostId);
                if (result)
                {
                    if (obj != null)
                    {
                        if (await log_repo.Remove((int)obj))
                        {
                            ListLog.Remove(ListLog.SingleOrDefault(t => t.LogID == (int)obj));
                            dc = new DialogContent() { Content = "Xóa Thành Công", Tilte = "Thông Báo" };
                            dialog = new DialogOk() { DataContext = dc };
                            await DialogHost.Show(dialog, DialogHostId);
                        }
                        else
                        {
                            dc = new DialogContent() { Content = "Xóa Thất Bại", Tilte = "Thông Báo" };
                            dialog = new DialogOk() { DataContext = dc };
                            await DialogHost.Show(dialog, DialogHostId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void CreateNewData()
        {
            try
            {
                LogID = Name = Fee = null;
                await DialogHost.Show(new LogisticProfile(), DialogHostId);
            }
            catch (Exception)
            {

                MessageBox.Show("có lỗi");
            }
        }

        private async void Submit(string obj)
        {
            try
            {
                var newObj = new Logistic()
                {
                    Name = Name,
                };
                newObj.Fee = Convert.ToDecimal(Fee);

                if (string.IsNullOrEmpty(obj))
                {

                    var result = await log_repo.Add(newObj);
                    if (result != null)
                    {
                        ListLog.Add(result);
                        dc = new DialogContent() { Content = "Thêm Thành Công", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);

                    }
                    else
                    {
                        dc = new DialogContent() { Content = "Thêm Thất Bại", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                    }
                }
                else
                {
                    newObj.LogID = Convert.ToInt32(LogID);
                    if (await log_repo.Update(newObj))
                    {
                        dc = new DialogContent() { Content = "Cập Thành Công", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                        ListLog = new ObservableCollection<Logistic>(log_repo.GetAll());
                    }
                    else
                    {
                        dc = new DialogContent() { Content = "Cập Thất Bại", Tilte = "Thông Báo" };
                        dialog = new DialogOk() { DataContext = dc };
                        DialogHost.CloseDialogCommand.Execute(null, null);
                        await DialogHost.Show(dialog, DialogHostId);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Có Lỗi");
            }
        }

        private void CreateData()
        {
            ListLog = new ObservableCollection<Logistic>(log_repo.GetAll());
            UpdateCommand = new DelegateCommand<int?>(LoadData);
        }

        private async void LoadData(int? obj)
        {
            if(obj!=null)
            {
                var i = ListLog.SingleOrDefault(t => t.LogID == (int)obj);
                if(i!=null)
                {
                    LogID = i.LogID.ToString();
                    Name = i.Name;
                    Fee = i.Fee.ToString();
                }

                await DialogHost.Show(new LogisticProfile(), DialogHostId);
            }
        }
    }
}
