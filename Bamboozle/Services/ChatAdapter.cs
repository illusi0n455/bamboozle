using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Text;

namespace Bamboozle
{
	public class ChatAdapter :  ArrayAdapter<MessageContent>
	{
		private List<MessageContent> _messageList;
		private Activity _context;
		public ChatAdapter(Activity context, List<MessageContent> messageList)
			: base(context, Resource.Layout.ChatBubble, messageList)
		{
			this._context = context;
			this._messageList = messageList;
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = this._messageList[position];
			var view = (convertView ??
				   this._context.LayoutInflater.Inflate(
				   Resource.Layout.ChatBubble,
				   parent,
				   false)) as LinearLayout;

			TextView username = view.FindViewById<TextView>(Resource.Id.txtBubbleFrom);
			TextView message = view.FindViewById<TextView>(Resource.Id.txtBubbleText);
			username.Text = item.From;
			message.Text = item.Text;
			//if (item.IsTheDeviceUser == false)
			//{
			//	view.SetGravity(GravityFlags.Left);
			//	message.SetBackgroundResource(Resource.Drawable.bubble_other);
			//}
			//else
			//{
			//	view.SetGravity(GravityFlags.Right);
			//	message.SetBackgroundResource(Resource.Drawable.bubble_user);
			//}

			return view;
		}
	}
}