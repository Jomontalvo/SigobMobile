﻿namespace SigobMobile.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Properties;
    public static class Languages
    {
        #region Constructors
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resources.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);

        }
        #endregion

        #region Languages Properties (Main Dictionary)

        #region Management Status (all componentes)
        public static string AllStatus => Resources.AllStatus;
        public static string CompletedStatus => Resources.CompletedStatus;
        public static string InManagementStatus => Resources.InManagementStatus;
        public static string ManagementStatusTitle => Resources.ManagementStatusTitle;
        public static string SuspendedStatus => Resources.SuspendedStatus;
        #endregion

        #region Filter for Confirmation Appointment Status
        public static string AppointmentColor => Resources.AppointmentColor;
        public static string CalendarFiltersMessage => Resources.CalendarFiltersMessage; 
        public static string ConfirmedFilterTitle => Resources.ConfirmedFilterTitle;
        public static string OnlyConfirmed => Resources.OnlyConfirmed;
        public static string OnlyTentative => Resources.OnlyTentative;
        public static string ConfirmedAndTentative => Resources.ConfirmedAndTentative;
        public static string TentativeLabel => Resources.TentativeLabel;
        public static string ShowCalendarColorOnEvent = Resources.ShowCalendarColorOnEvent;
        #endregion

        #region Time Measures
        public static string AllDay => Resources.AllDay;
        public static string Hour => Resources.Hour;
        public static string Hours => Resources.Hours;
        public static string Day => Resources.Day;
        public static string Days => Resources.Days;
        public static string Month => Resources.Month;
        public static string Months => Resources.Months;
        public static string Year => Resources.Year;
        public static string Years => Resources.Years;
        #endregion

        #region Deadline Status (all components)
        public static string InProgressStatus => Resources.InProgressStatus;
        public static string PendingStatus => Resources.PendingStatus;
        public static string CloseToDeadlinedStatus => Resources.CloseToDeadlinedStatus;
        public static string ExpiredStatus => Resources.ExpiredStatus;
        public static string OverdueStatus => Resources.OverdueStatus;
        #endregion

        #region Search Tools
        public static string SearchExternalContacts => Resources.SearchExternalContacts;
        public static string SearchPlaceHolder => Resources.SearchPlaceHolder;
        public static string SearchInternalContactsPlaceHolder => Resources.SearchInternalContactsPlaceHolder;
        public static string SearchExternalContactsPlaceHolder => Resources.SearchExternalContactsPlaceHolder;
        public static string SearchDeviceContactsPlaceHolder => Resources.SearchDeviceContactsPlaceHolder;
        public static string Filters => Resources.Filters;
        public static string SelectAll => Resources.SelectAll;
        public static string UnselectAll => Resources.UnselectAll;
        public static string ShowAll => Resources.ShowAll;
        public static string HideAll => Resources.HideAll;
        #endregion

        #region Dialog Box
        public static string Accept => Resources.Accept;
        public static string Cancel => Resources.Cancel;
        public static string Error => Resources.Error;
        public static string Ok => Resources.Ok;
        public static string Yes => Resources.Yes;
        public static string No => Resources.No;
        public static string Success => Resources.Success;
        public static string GeneralError => Resources.GeneralError;
        public static string UnauthorizedError => Resources.UnauthorizedError;
        public static string Add => Resources.Add;
        public static string Edit => Resources.Edit;
        public static string Save => Resources.Save;
        public static string Done => Resources.Done;
        public static string Delete => Resources.Delete;
        public static string Confirm => Resources.ConfirmTitle;
        public static string ConfirmEventDeletion => Resources.ConfirmEventDeletion;
        #endregion

        #region Await Messages
        public static string Loading => Resources.Loading;
        #endregion

        #region Login View
        public static string LoginViewTitle => Resources.LoginViewTitle;
        public static string EnterCredentials => Resources.EnterCredentials;
        public static string Username => Resources.Username;
        public static string Password => Resources.Password;
        public static string SelectApiInstitution => Resources.SelectApiInstitution;
        public static string Login => Resources.Login;
        public static string ForgotPassword => Resources.ForgotPassword;
        //Messages
        public static string InvalidCredentials => Resources.InvalidCredentials;
        public static string UserValidationMsg => Resources.UserValidationMsg;
        public static string PasswordValidationMsg => Resources.PasswordValidationMsg;
        #endregion

        #region Institutions to connect View
        public static string InstitutionsConnect => Resources.InstitutionsConnect;
        #endregion

        #region Change Password View
        public static string ChangePasswordTitle => Resources.ChangePasswordTitle;
        public static string NewPassword => Resources.NewPassword;
        public static string RepeatNewPassword => Resources.RepeatNewPassword;
        public static string PasswordsMatchValidationMsg => Resources.PasswordsMatchValidationMsg;
        public static string NewPasswordValidationMsg => Resources.NewPwdValidationMsg;
        public static string RepeatNewPasswordValidationMsg => Resources.RepeatNewPwdValidationMsg;
        public static string ChangePasswordText => Resources.ChangePasswordText;
        #endregion

        #region Main Menu Options
        public static string MasterPageTitle => Resources.MasterPageTitle;
        public static string HomeMenu => Resources.HomeMenu;
        public static string HelpMenu => Resources.HelpMenu;
        public static string SecurityMenu => Resources.SecurityMenu;
        public static string TermsAndConditionsMenu => Resources.TermsAndConditionsMenu;
        public static string ContactUs => Resources.ContactUs;
        public static string LogoutMenu => Resources.LogoutMenu;
        #endregion

        #region Applications Menu View
        public static string ManagementCenterAppName => Resources.ManagementCenterAppName;
        public static string TaskManagementAppName => Resources.TaskManagementAppName;
        public static string InstitutionalGoalsAppName => Resources.InstitutionalGoalsAppName;
        public static string CorrespondenceAppName => Resources.CorrespondenceAppName;
        public static string WorkflowProcessAppName => Resources.WorkflowProcessAppName;
        public static string CommunicationsActionsAppName => Resources.CommunicationsActionsAppName;
        #endregion

        #region My Profile View
        public static string ProfileViewTitle => Resources.ProfileViewTitle;
        public static string PersonalDataGroup => Resources.PersonalDataGroup;
        public static string OfficialDataGroup => Resources.OfficialDataGroup;
        public static string ContactDataGroup => Resources.ContactDataGroup;
        public static string FirstNameText => Resources.FirstNameText;
        public static string LastNameText => Resources.LastNameText;
        public static string InstitutionText => Resources.InstitutionText;
        public static string AreaText => Resources.AreaText;
        public static string PositionText => Resources.PositionText;
        public static string PhoneText => Resources.PhoneText;
        public static string CellPhoneText => Resources.CellPhoneText;
        public static string EmailText => Resources.EmailText;
        public static string GenderText => Resources.GenderText;
        public static string SelectGenderPlaceHolder => Resources.SelectGenderPlaceHolder;
        public static string MaleGender => Resources.MaleGender;
        public static string FemaleGender => Resources.FemaleGender;
        public static string UserImageText => Resources.UserImageText;
        public static string SourceImageQuestion => Resources.SourceImageQuestion;
        public static string FromCamera => Resources.FromCamera;
        public static string FromGallery => Resources.FromGallery;
        public static string DeletePicture => Resources.DeletePicture;

        #endregion

        #region Calendar Agenda View
        public static string AgendaViewTitle => Resources.AgendaViewTitle;
        public static string CalendarsTitle => Resources.CalendarsTitle;
        public static string Calendar => Resources.Calendar;
        public static string Today => Resources.Today;
        public static string Yesterday => Resources.Yesterday;
        public static string CalendarViewModeText => Resources.CalendarViewModeText;
        public static string DailyView => Resources.DailyView;
        public static string MonthlyView => Resources.MonthlyView;
        public static string MultiDayView => Resources.MultiDayView;
        public static string YearView => Resources.YearView;
        public static string WeeklyView => Resources.WeeklyView;
        public static string SelectCalendarMessage => Resources.SelectCalendarMessage;
        public static string SelectCalendarColor => Resources.SelectCalendarColor;
        public static string SelectItemToAddManagementCenter => Resources.SelectItemToAddManagementCenter;
        public static string ManagementCenters => Resources.ManagementCenters;
        public static string PersonalAgendas => Resources.PersonalAgendas;
        #endregion

        #region Event View
        public static string EventNameText => Resources.EventNameText;
        public static string EventLocationText => Resources.EventLocationText;
        public static string EventStartDateText => Resources.EventStartText;
        public static string EventEndDateText => Resources.EventEndText;
        public static string EventTimeStartText => Resources.EventTimeStartText;
        public static string EventTimeEndText => Resources.EventTimeEndText;
        public static string EventCalendarOwner => Resources.EventCalendarOwner;
        public static string EventTypeText => Resources.EventTypeText;
        public static string EventStatusText => Resources.EventStatusText;
        public static string EventProgrammerText => Resources.EventProgrammerText;
        public static string EventSummaryText => Resources.EventSummaryText;
        public static string EventParticipantText => Resources.EventParticipantText;
        public static string EventParticipantsText => Resources.EventParticipantsText;
        public static string EventAbstractText => Resources.EventAbstractText;
        public static string EventAnnotationsText => Resources.EventAnnotationsText;
        public static string EventFinishCommand => Resources.EventFinishCommand;
        public static string EventSuspendCommand => Resources.EventSuspendCommand;
        public static string EventActivateCommand => Resources.EventActivateCommand;
        public static string EventFinishMessage => Resources.EventFinishMessage;
        public static string EventResumeMessage => Resources.EventResumeMessage;
        public static string EventSuspendMessage => Resources.EventSuspendMessage;
        #endregion

        #region Helper text
        public static string Me => Resources.Me;
        public static string And => Resources.And;
        #endregion

        #region Confidentiality Level of Management Center Events
        public static string NoConfidentialityLevel => Resources.NoConfidentialityLevel;
        public static string PublicConfidentialityLevel => Resources.PublicConfidentialityLevel;
        public static string LowConfidentialityLevel => Resources.LowConfidentialityLevel;
        public static string MediumConfidentialityLevel => Resources.MediumConfidentialityLevel;
        public static string HighConfidentialityLevel => Resources.HighConfidentialityLevel;
        public static string PrivateConfidentialityLevel => Resources.PrivateConfidentialityLevel;
        #endregion

        #region Appointment View
        public static string AppointmentViewTitle => Resources.AppointmentViewTitle;
        #endregion

        #region Management Center Items
        public static string Event => Resources.Event;
        public static string Events => Resources.Events;
        public static string Activity => Resources.Activity;
        public static string Activities => Resources.Activities;
        public static string Appointment => Resources.Appointment;
        public static string Appointments => Resources.Appointments;
        public static string Instruction => Resources.Instruction;
        public static string Instructions => Resources.Instructions;
        public static string Assignment => Resources.Assignment;
        public static string Assignments => Resources.Assignments;
        public static string Task => Resources.Task;
        public static string Tasks => Resources.Tasks;
        public static string Request => Resources.Request;
        public static string Requests => Resources.Requests;
        public static string TaskPrevious => Resources.TaskPrevious;
        public static string TaskSupport => Resources.TaskSupport;
        public static string TaskCommitments => Resources.TaskCommitments;
        #endregion

        #region Institutional Directory and Contacts
        public static string InstitutionalDirectoryTitle => Resources.InstitutionalDirectoryTitle;
        public static string ContactsTitle => Resources.ContactsTitle;
        public static string ExternalContactsViewTitle => Resources.ExternalContactsViewTitle;
        #endregion

        #region Task Management View
        public static string MyTasks => Resources.MyTasks;
        public static string TasksOf => Resources.TasksOf;
        public static string PendingTasks => Resources.PendingTasks;
        public static string OverdueTasks => Resources.OverdueTasks;
        public static string CompletedTasks => Resources.CompletedTasks;
        public static string NewAssignedTasks => Resources.NewAssignedTasks;
        public static string OngoingTaskSetByMe => Resources.OngoingTaskSetByMe;
        public static string OngoingTasksThatMonitoring => Resources.OngoingTasksThatMonitoring;
        public static string CompletedTasksSetByMe => Resources.CompletedTasksSetByMe;
        public static string CompletedTasksThatMonitoring => Resources.CompletedTasksThatMonitoring;
        public static string TaskCopies => Resources.TaskCopies;
        public static string TaskMessages => Resources.TaskMessages;
        public static string TaskCloseToDeadline => Resources.TaskCloseToDeadline;
        public static string TaskControl => Resources.TaskControl;
        public static string ControlTitle => Resources.ControlTitle;
        public static string PersonalControl => Resources.PersonalControl;
        public static string GeneralControl => Resources.GeneralControl;
        public static string OverdueControl => Resources.OverdueControl;
        public static string EmptyTaskList => Resources.EmptyTaskList;
        public static string TaskProgrammerText => Resources.TaskProgrammerText;
        public static string OverdueTasksByType => Resources.OverdueTasksByType;
        public static string OverdueTasksByProgrammer => Resources.OverdueTasksByProgrammer;
        public static string OverdueTasksByResponsible => Resources.OverdueTasksByResponsible;
        #endregion

        #region Report View
        public static string ReportPeriodicity => Resources.ReportPeriodicity;
        public static string PeriodicityUndefined => Resources.PeriodicityUndefined;
        public static string PeriodicityWeekly => Resources.PeriodicityWeekly;
        public static string PeriodicityBiWeekly => Resources.PeriodicityBiWeekly;
        public static string PeriodicityMonthly => Resources.PeriodicityMonthly;
        public static string PeriodicityBiMonthly => Resources.PeriodicityBiMonthly;
        public static string NewReport => Resources.NewReport;
        public static string NoReportAvailable => Resources.NoReportAvailable;
        #endregion

        #region Messages
        public static string Messages => Resources.Messages;
        public static string Message => Resources.Message;
        public static string NewMessage => Resources.NewMessage;
        public static string SendMessage => Resources.SendMessage;
        #endregion

        #region Correspondence Menu View
        public static string ManagementTray = Resources.ManagementTray;
        public static string TagTray = Resources.TagTray;
        public static string External = Resources.External;
        public static string Internal = Resources.Internal;
        public static string Draft = Resources.Draft;
        public static string ProcessedToConfirm = Resources.ProcessedToConfirm;
        public static string Pending = Resources.Pending;
        public static string ForYourInformation = Resources.ForYourInformation;
        public static string Trasferred = Resources.Trasferred;
        public static string WithDeadline = Resources.WithDeadline;
        public static string PendingEmail = Resources.PendingEmail;
        public static string PendingLinkedInstitution = Resources.PendingLinkedInstitution;
        #endregion

        #region Pagination Info
        public static string TotalRows = Resources.TotalRows;
        public static string ShowRowCount = Resources.ShowRowCount;
        #endregion

        #region Search Correspondence
        public static string Search = Resources.Search;
        public static string SearchFilters = Resources.SearchFilters;
        public static string Sender = Resources.Sender;
        public static string Receiver = Resources.Receiver;
        public static string Anytime = Resources.Anytime;
        public static string ThisWeek = Resources.ThisWeek;
        public static string LastWeek = Resources.LastWeek;
        public static string LastTwoWeeks = Resources.LastTwoWeeks;
        public static string LastMonth = Resources.LastMonth;
        public static string LastTrimester = Resources.LastTrimester;
        public static string LastSemester = Resources.LastSemester;
        public static string LastYear = Resources.LastYear;
        #endregion

        #region Correspondence Details
        public static string From = Resources.From;
        public static string To = Resources.To;
        public static string Copy = Resources.Copy;
        public static string Copies = Resources.Copies;
        public static string Senders = Resources.Senders;
        public static string Receivers = Resources.Receivers ;
        public static string ScannedDocuments = Resources.ScannedDocuments;
        public static string ScannedDocument = Resources.ScannedDocument;
        public static string NoScannedDocuments = Resources.NoScannedDocuments;
        public static string Tracking = Resources.Tracking;
        public static string LinkedDocuments = Resources.LinkedDocuments;
        public static string NoLinkedDocuments = Resources.NoLinkedDocuments;
        public static string InKnowledgeOne = Resources.InKnowledgeOne;
        public static string InKnowledgeSeveral = Resources.InKnowledgeSeveral;
        public static string NoOneInformed = Resources.NoOneInformed;
        public static string DocumentPurpose =  Resources.DocumentPurpose;
        public static string Objective = Resources.Objective;
        public static string Objectives = Resources.Objectives;
        public static string Result = Resources.Result;
        public static string Results = Resources.Results;
        public static string TransferredBy = Resources.TransferredBy;
        public static string NoTags = Resources.NoTags;
        #endregion

        #region Documents Attachments
        public static string Attachment = Resources.Attachment;
        public static string Attachments = Resources.Attachments;
        public static string NoAttachments = Resources.NoAttachments;
        public static string SelectApp = Resources.SelectApp;
        public static string Folder = Resources.Folder;
        public static string DefaultFolder = Resources.DefaultFolder;
        #endregion

        #region Document Trace
        public static string TrackingViewTitle = Resources.TrackingViewTitle;
        public static string OfficialStart = Resources.OfficialStart;
        public static string CurrentOfficial = Resources.CurrentOfficial;
        public static string OfficialEnd = Resources.OfficialEnd;

        #endregion

        #region Reply Status
        public static string NotReplyRequired = Resources.NotReplyRequired;
        public static string NotReplied = Resources.NotReplied;
        public static string Replied = Resources.Replied;
        #endregion

        #endregion
    }
}
