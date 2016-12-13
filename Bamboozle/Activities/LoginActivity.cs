using Android.App;
using Android.Gms.Tasks;
using Android.OS;
using Android.Widget;
using Firebase.Auth;
using Android.Views;

namespace Bamboozle
{
	[Activity(Label = "LoginActivity", Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
	public class LoginActivity : Activity, IOnCompleteListener
	{
		private Button btnLogin;
		private Button btnSignUp;
		private EditText etxtEmail;
		private EditText etxtPassword;
		private TextView txtError;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Login);
			btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
			btnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
			etxtEmail = FindViewById<EditText>(Resource.Id.etxtEmail);
			etxtPassword = FindViewById<EditText>(Resource.Id.etxtPassword);
			txtError = FindViewById<TextView>(Resource.Id.txtError);
			btnLogin.Click += delegate
			{
				Login(etxtEmail.Text, etxtPassword.Text);
			};
			btnSignUp.Click += delegate
			{
				StartActivity(typeof(SignUpActivity));
				Finish();
			};
		}
		private void Login(string email, string password)
		{
			FirebaseAuth.Instance.SignInWithEmailAndPassword(email, password).AddOnCompleteListener(this);
		}
		public void OnComplete(Task task)
		{
			if (task.IsSuccessful)
			{
				StartActivity(typeof(MainActivity));
				Finish();
			}
			else
			{
				txtError.Text = "You specified wrong credentials";
				txtError.Visibility = ViewStates.Visible;
			}
		}
	}
}