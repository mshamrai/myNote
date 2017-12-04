﻿using myNote.ClientService;
using myNote.Model;
using myNote.WPFClient.DataModels;
using myNote.WPFClient.View;
using myNote.WPFClient.View.Pages.MainFrameContent;
using myNote.WPFClient.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace myNote.WPFClient.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        #region Public Properties

        #region ListViews properties
        /// <summary>
        /// User's notes
        /// </summary>
        public ObservableCollection<Note> Notes { get; set; }
        /// <summary>
        /// Shared notes
        /// </summary>
        public ObservableCollection<Note> SharedNotes { get; set; }
        /// <summary>
        /// User's groups
        /// </summary>
        public ObservableCollection<Group> Groups { get; set; }
        /// <summary>
        /// User's note groups 
        /// </summary>
        public ObservableCollection<NoteGroup> NoteGroups { get; set; }
        /// <summary>
        /// Selected listview note
        /// </summary>
        public Note SelectedNote { get; set; }
        #endregion

        #region Frame properties
        /// <summary>
        /// Frame content page
        /// </summary>
        public Page CurrentContentPage { get; set; }

        #endregion

        #endregion

        #region Commands
        /// <summary>
        /// Select note command
        /// </summary>
        public ICommand SelectedNoteCommand { get; set; }
        /// <summary>
        /// This command execute when page loaded
        /// </summary>
        public ICommand LoadPageCommand { get; set; }
        /// <summary>
        /// Add note menu item click command
        /// </summary>
        public ICommand AddNoteCommand { get; set; }
        /// <summary>
        /// The command execute when shared tab item is selected
        /// </summary>
        public ICommand SharedNoteEnterCommand { get; set; }
        /// <summary>
        /// The command execute when user choose group to show
        /// </summary>
        public ICommand ChooseGroupCommand { get; set; }
        /// <summary>
        /// The command execute when user pick all button
        /// </summary>
        public ICommand ShowAllNote { get; set; }
        /// <summary>
        /// Add group menu item click command
        /// </summary>
        public ICommand AddGroupCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainPageViewModel()
        {
            var api = ClientService.ApiClient.CreateInstance(IoC.IoC.ConnectionString);

            AddGroupCommand = new RelayCommand(async(obj) =>
            {
                string name;
                var dialog = new InputWindow();
                if (dialog.ShowDialog() == true)
                {
                    name = dialog.Result;
                    Groups.Add(await api.GroupService.CreateGroupAsync(name, UserData.UserDataContent.Token));
                }
            });

            ShowAllNote = new RelayCommand(async (obj) =>
            {
                try
                {                    
                    Notes = new ObservableCollection<Note>(await api.NoteService.GetUserNotesAsync(UserData.UserDataContent.Token.UserId));
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            ChooseGroupCommand = new RelayCommand(async (obj) =>
            {
                try
                {
                    Notes = new ObservableCollection<Note>(await api.NoteGroupService.GetNotesInGroupAsync(obj as string, UserData.UserDataContent.Token));
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            AddNoteCommand = new RelayCommand((obj) =>
            {
                CurrentContentPage = new NoteContentPage(new Note { UserId = UserData.UserDataContent.Token.UserId });
            });

            SelectedNoteCommand = new RelayCommand((obj) =>
            {
                CurrentContentPage = new NoteContentPage(SelectedNote);
            });

            LoadPageCommand = new RelayCommand(async(obj) =>
            {
                try
                {
                    Groups = new ObservableCollection<Group>(await api.UserService.GetUserGroupsAsync(UserData.UserDataContent.Token.UserId, UserData.UserDataContent.Token));                    
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            SharedNoteEnterCommand = new RelayCommand(async (obj) =>
            {
                try
                {
                    SharedNotes = new ObservableCollection<Note>(await api.ShareService.GetSomeShares());
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
        #endregion
    }
}
