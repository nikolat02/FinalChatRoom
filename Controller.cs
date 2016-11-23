using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class ClientController
    {
        //Notice we only use the interfaces. This makes the test more 
        //robust to changes in the system.
        IUsersView _view;
        IList _users;
        User _selectedUser;

        //The UsersController depends on abstractions(interfaces).
        //It's easier than ever to change the behavior of a concrete class. 
        //Instead of creating concrete objects in UsersController class, 
        //we pass the objects to the constructor of UsersController
        public UsersController(IUsersView view, IList users)
        {
            _view = view;
            _users = users;
            view.SetController(this);
        }

        public IList Users
        {
            get { return ArrayList.ReadOnly(_users); }
        }

        private void updateViewDetailValues(User usr)
        {
            _view.FirstName = usr.FirstName;
            _view.LastName = usr.LastName;
            _view.ID = usr.ID;
            _view.Department = usr.Department;
            _view.Sex = usr.Sex;
        }

        private void updateUserWithViewValues(User usr)
        {
            usr.FirstName = _view.FirstName;
            usr.LastName = _view.LastName;
            usr.ID = _view.ID;
            usr.Department = _view.Department;
            usr.Sex = _view.Sex;
        }

        public void LoadView()
        {
            _view.ClearGrid();
            foreach (User usr in _users)
                _view.AddUserToGrid(usr);

            _view.SetSelectedUserInGrid((User)_users[0]);
        }

        public void SelectedUserChanged(string selectedUserId)
        {
            foreach (User usr in this._users)
            {
                if (usr.ID == selectedUserId)
                {
                    _selectedUser = usr;
                    updateViewDetailValues(usr);
                    _view.SetSelectedUserInGrid(usr);
                    this._view.CanModifyID = false;
                    break;
                }
            }
        }

        public void AddNewUser()
        {
            _selectedUser = new User(""   /*firstname*/,
            ""  /*lastname*/,
            "" /*id*/,
            ""/*department*/,
            User.SexOfPerson.Male/*sex*/);

            this.updateViewDetailValues(_selectedUser);
            this._view.CanModifyID = true;
        }

        public void RemoveUser()
        {
            string id = this._view.GetIdOfSelectedUserInGrid();
            User userToRemove = null;

            if (id != "")
            {
                foreach (User usr in this._users)
                {
                    if (usr.ID == id)
                    {
                        userToRemove = usr;
                        break;
                    }
                }

                if (userToRemove != null)
                {
                    int newSelectedIndex = this._users.IndexOf(userToRemove);
                    this._users.Remove(userToRemove);
                    this._view.RemoveUserFromGrid(userToRemove);

                    if (newSelectedIndex > -1 && newSelectedIndex < _users.Count)
                    {
                        this._view.SetSelectedUserInGrid((User)_users[newSelectedIndex]);
                    }
                }
            }
        }

        public void Save()
        {
            updateUserWithViewValues(_selectedUser);
            if (!this._users.Contains(_selectedUser))
            {
                //Add new user
                this._users.Add(_selectedUser);
                this._view.AddUserToGrid(_selectedUser);
            }
            else
            {
                //Update existing user
                this._view.UpdateGridWithChangedUser(_selectedUser);
            }
            _view.SetSelectedUserInGrid(_selectedUser);
            this._view.CanModifyID = false;

        }
    