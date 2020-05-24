using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.TIModels;
using Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.Services;
using Pithline.WinRT.Components.Controls.WindowsPhone;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.ViewModels
{
    public class InspectionDetailPageViewModel : ViewModel
    {

        private TITask _task;
        private INavigationService _navigationService;
        private ITaskService _taskService;
        private BusyIndicator _busyIndicator;
        public InspectionDetailPageViewModel(INavigationService navigationService, ITaskService taskService)
        {
            this._navigationService = navigationService;
            this._taskService = taskService;
            this.Model = new TIData();

            _busyIndicator = new BusyIndicator();
            BoundWidth = Window.Current.Bounds.Width - 60;
            BoundHeight = (Window.Current.Bounds.Height - 100) / 3;
            CompleteCommand = new DelegateCommand(async () =>
            {
                try
                {
                    _busyIndicator.Open("Please wait, Saving ...");
                    this.SelectedTask.Status = Pithline.FMS.BusinessLogic.Portable.SSModels.DriverTaskStatus.Completed;
                    var imageCaptureList = await Util.ReadFromDiskAsync<List<Pithline.FMS.BusinessLogic.Portable.TIModels.ImageCapture>>("ImageCaptureList");
                    var resp = await this._taskService.InsertInspectionDataAsync(new List<TIData> { this.Model }, this.SelectedTask, imageCaptureList, UserInfo.CompanyId);

                    if (resp)
                    {
                        PersistentData.RefreshInstance();
                        _navigationService.Navigate("Main", string.Empty);
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    _busyIndicator.Close();

                }

            });
            this.VoiceCommand = new DelegateCommand<string>(async (param) =>
            {

                using (SpeechRecognizer recognizer = new SpeechRecognizer())
                {
                    SpeechRecognitionTopicConstraint topicConstraint
                     = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "Development");

                    recognizer.Constraints.Add(topicConstraint);
                    await recognizer.CompileConstraintsAsync();

                    var results = await recognizer.RecognizeWithUIAsync();

                    if (results != null & (results.Confidence != SpeechRecognitionConfidence.Rejected))
                    {
                        if (param == "Remedy")
                        {
                            this.Model.Remedy = results.Text;
                        }
                        if (param == "Recommendation")
                        {
                            this.Model.Recommendation = results.Text;
                        }
                        if (param == "CauseOfDamage")
                        {
                            this.Model.CauseOfDamage = results.Text;
                        }
                    }

                    else
                    {
                        await new MessageDialog("Sorry, I did not get that.").ShowAsync();
                    }
                }

            });

        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
                {
                    this.UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
                }

                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.SELECTEDTASK))
                {
                    this.SelectedTask = JsonConvert.DeserializeObject<Pithline.FMS.BusinessLogic.Portable.TIModels.TITask>(ApplicationData.Current.RoamingSettings.Values[Constants.SELECTEDTASK].ToString());
                }
                if (this.SelectedTask != null)
                {
                    this.Model.CaseServiceRecID = this.SelectedTask.CaseServiceRecID;
                }

            }
            catch (Exception)
            {

            }
        }

        public Pithline.FMS.BusinessLogic.Portable.TIModels.TITask SelectedTask { get; set; }

        private TIData model;
        public TIData Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private double boundWidth;
        public double BoundWidth
        {
            get { return boundWidth; }
            set { SetProperty(ref boundWidth, value); }
        }

        private double boundHeight;
        public double BoundHeight
        {
            get { return boundHeight; }
            set { SetProperty(ref boundHeight, value); }
        }

        public Pithline.FMS.BusinessLogic.Portable.TIModels.UserInfo UserInfo { get; set; }
        public DelegateCommand CompleteCommand { get; set; }
        public DelegateCommand<string> VoiceCommand { get; set; }
    }
}