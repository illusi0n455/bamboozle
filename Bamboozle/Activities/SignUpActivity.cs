using Android.App;
using Android.OS;
using Android.Widget;
using Firebase.Auth;
using Android.Gms.Tasks;
using System;
using Android.Views;

namespace Bamboozle
{
	[Activity(Label = "SignUpActivity", Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
	public class SignUpActivity : Activity, IOnCompleteListener
	{
		private Button btnSubmit;
		private EditText etxtEmail;
		private EditText etxtPassword;
		private EditText etxtName;
		private TextView txtError;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.SignUp);
			btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
			etxtEmail = FindViewById<EditText>(Resource.Id.etxtEmail);
			etxtPassword = FindViewById<EditText>(Resource.Id.etxtPassword);
			etxtName = FindViewById<EditText>(Resource.Id.etxtName);
			txtError = FindViewById<TextView>(Resource.Id.txtError);
			btnSubmit.Click += delegate
			{
				SignUp(etxtEmail.Text, etxtPassword.Text);
			};
		}

		private void SignUp(string email, string password)
		{
			FirebaseAuth.Instance.CreateUserWithEmailAndPassword(email, password).AddOnCompleteListener(this);
		}

		public void OnComplete(Task task)
		{
			if (task.IsSuccessful)
			{
				//FirebaseService.Client.Child("users").PostAsync(new MessageContent("testuser", txtMessage.Text));
				StartActivity(typeof(LoginActivity));
				Finish();
			}
			else
			{
				if (etxtPassword.Text.Length<8)
				{
					txtError.Text = "Password should contain at least 8 characters";
				}
				else
				{
					txtError.Text = "Specified email is already in use";
				}
				txtError.Visibility = ViewStates.Visible;
			}
		}
	}
}