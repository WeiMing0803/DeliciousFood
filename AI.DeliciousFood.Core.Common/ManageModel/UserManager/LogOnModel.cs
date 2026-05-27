namespace AI.DeliciousFood.Core.Common.ManageModel.UserManager;

public record LogOnModel(string Username, string Password);

public record LogOnResponse(int Code, string Msg);
