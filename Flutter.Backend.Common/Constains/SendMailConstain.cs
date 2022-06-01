namespace Flutter.Backend.Common.Constains
{
    public static class SendMailConstain
    {
        public const string EmailComfirmUrl = "http://htcwatch-api.herokuapp.com/api/authenticate/comfirm-email/{0}";

        public const string EmailResetPasswordUrl = "http://htcwatch-api.herokuapp.com/api/authenticate/send-email/reset-password";

        public const string TemplateEmailRegister = "TEMPLATE_EMAIL_REGISTER_ACCOUNT";

        public const string TemplateEmailConfirm = "TEMPLATE_EMAIL_CONFIRM_ORDER";

        public const string TemplateEmailCancle = "TEMPLATE_EMAIL_CANCLE_ORDER";

        public const string TemplateEmailDelivery = "TEMPLATE_EMAIL_DELIVERY_ORDER";

        public const string TemplateEmailSuccess = "TEMPLATE_EMAIL_SUCCESS_ORDER";

        public const string TemplateEmailResetPassword = "TEMPLATE_EMAIL_RESET_EMAIL";

        public const string SubjectRegister = "Đăng Ký Tài Khoản Cho Ứng Dụng HTC";

        public const string SubjectResetPassword = "Lấy Lại Mật Khẩu Cho Tài Khoản Ứng Dụng HTC";
    }
}
