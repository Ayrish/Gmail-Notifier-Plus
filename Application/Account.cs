﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using GmailNotifierPlus.Utilities;

namespace GmailNotifierPlus {

	public enum AccountTypes {
		Regular,
		GoogleApps
	}

	[DataContract(Name = "account")]
	public class Account {

		public event AccountChangedEventHandler AccountChanged;

		public static class Domains {
			public const String Gmail = "gmail.com";
			public const String GmailAlt = "googlemail.com";
			public const String GmailUK = "googlemail.co.uk";
			public const String WindowTitle = "Gmail Notifier Plus";
		}

		private String _login = String.Empty;
		private String _password = String.Empty;
		private String _consumerKey = String.Empty;
		private String _consumerSecret = String.Empty;

		public Account() {
			this.Login = this.Password = this.Name = this.Domain = this.FullAddress = String.Empty;
			this.Type = AccountTypes.Regular;
			this.Emails = new List<Email>();
			this.Guid = System.Guid.NewGuid().ToString();
		}

		public Account(String login, String password) {

			this.Login = login;
			this.Password = password;
			this.Guid = System.Guid.NewGuid().ToString();

			Init();
		}

		public String Guid { get; private set; }

		public String Login {
			get { return _login; }
			set {
				String original = _login;
				_login = value;

				if (original != _login) {
					Init();
					if (AccountChanged != null) {
						AccountChanged(this);
					}
				}
			}
		}

		public String Password {
			get { return _password; }
			set {
				String original = _password;
				_password = value;

				if (original != _password && AccountChanged != null) {
					AccountChanged(this);
				}
			}
		}

		public String ConsumerKey {
			get { return _consumerKey; }
			set {
				String original = _consumerKey;
				_consumerKey = value;

				if(original != _consumerKey && AccountChanged != null) {
					AccountChanged(this);
				}
			}
		}

		public String ConsumerSecret {
			get { return _consumerSecret; }
			set {
				String original = _consumerSecret;
				_consumerSecret = value;

				if(original != _consumerSecret && AccountChanged != null) {
					AccountChanged(this);
				}
			}
		}

		[DataMember(Name = "default")]
		public Boolean Default { get; set; }

		[DataMember(Name = "mailto")]
		public Boolean HandlesMailto { get; set; }

		public String Domain { get; private set; }
		public List<Email> Emails { get; private set; }
		public String FullAddress { get; private set; }
		public String Name { get; private set; }
		public AccountTypes Type { get; private set; }
		public int Unread { get; set; }

		/// <summary>
		/// Null indicates that we should use the system default.
		/// </summary>
		[DataMember(Name = "browser")]
		public Shellscape.Browser Browser { get; set; }

		[DataMember(Name = "ahead")]
		private String LoginEncrypted {
			get {
				return EncryptionHelper.Encrypt(this.Login);
			}
			set {
				this.Login = EncryptionHelper.Decrypt(value);
			}
		}

		[DataMember(Name = "aft")]
		private String PasswordEncrypted {
			get {
				return EncryptionHelper.Encrypt(this.Password);
			}
			set {
				this.Password = EncryptionHelper.Decrypt(value);
			}
		}

		public void Init() {

			if (this.Emails == null) {
				this.Emails = new List<Email>();
			}
			else {
				this.Emails.Clear();
			}

			if (Guid == null) {
				this.Guid = System.Guid.NewGuid().ToString();
			}

			if (!String.IsNullOrEmpty(this.Login)) {
				String[] strArray = this.Login.Split(new char[] { '@' });

				if ((strArray.Length > 1) && !String.IsNullOrEmpty(strArray[1])) {
					this.Name = strArray[0];
					this.Domain = strArray[1];
				}
				else {
					this.Name = this.Login;
					this.Domain = Domains.Gmail;
				}

				if (((this.Domain == Domains.Gmail) || (this.Domain == Domains.GmailAlt)) || (this.Domain == Domains.GmailUK)) {
					this.Type = AccountTypes.Regular;
				}
				else {
					this.Type = AccountTypes.GoogleApps;
				}

				this.FullAddress = this.Name + "@" + this.Domain;
			}
		}

		public bool IsEmpty {
			get {
				if (!String.IsNullOrEmpty(this.Login)) {
					return String.IsNullOrEmpty(this.Password);
				}
				return true;
			}
		}


	}
}

