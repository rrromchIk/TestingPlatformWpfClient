namespace TestingPlatformWpfClient.Models {
    public class UpdateConfirmationDialogResult {
        public bool IsConfirmed { get; private set; }
        public Test UpdatedTest { get; private set; }

        private UpdateConfirmationDialogResult(bool isConfirmed, Test updatedTest = null) {
            IsConfirmed = isConfirmed;
            UpdatedTest = updatedTest;
        }

        public static UpdateConfirmationDialogResult Confirmed(Test updatedTest) {
            return new UpdateConfirmationDialogResult(true, updatedTest);
        }

        public static UpdateConfirmationDialogResult Cancelled() {
            return new UpdateConfirmationDialogResult(false);
        }
    }
}
