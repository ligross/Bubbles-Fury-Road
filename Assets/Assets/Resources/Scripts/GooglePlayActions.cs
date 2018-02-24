using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


public static class GooglePlayActions {
	// Acievments
	public const string pioneerAchiev = "CgkIqe6wvP0aEAIQAw";
	public const string lordAchiev = "CgkIqe6wvP0aEAIQBA";
	public const string flySwatAchiev = "CgkIqe6wvP0aEAIQBQ";
	public const string superhumanAchiev = "CgkIqe6wvP0aEAIQBg";
	public const string tricksterAchiev = "CgkIqe6wvP0aEAIQBw";
	public const string goodPupilAchiev = "CgkIqe6wvP0aEAIQCA";
	public const string climberAchiev = "CgkIqe6wvP0aEAIQCQ";
	public const string soLovelyAchiev = "CgkIqe6wvP0aEAIQCg";
	public const string scrapThisAchiev = "CgkIqe6wvP0aEAIQCw";
	public const string longLiveAchiev = "CgkIqe6wvP0aEAIQDA";
	public const string narcissistAchiev = "CgkIqe6wvP0aEAIQDQ";
	public const string doingSomethingAchiev = "CgkIqe6wvP0aEAIQDg";
	public const string bitNotEnoughAchiev = "CgkIqe6wvP0aEAIQDw";
	public const string prettyGoodAchiev = "CgkIqe6wvP0aEAIQEA";
	public const string flyBaronAchiev = "CgkIqe6wvP0aEAIQEg";
	public const string flyOnTheWallAchiev = "CgkIqe6wvP0aEAIQEw";
	public const string destrudoAchiev = "CgkIqe6wvP0aEAIQFA";
	public const string apathyAchiev = "CgkIqe6wvP0aEAIQFQ";
	public const string davidBlaineAchiev = "CgkIqe6wvP0aEAIQFg";
	public const string discordantAchiev = "CgkIqe6wvP0aEAIQFw";
	public const string tetractisAchiev = "CgkIqe6wvP0aEAIQGA";
	public const string beginnerAdsAchiev = "CgkIqe6wvP0aEAIQGQ";
	public const string intermAdsAchiev = "CgkIqe6wvP0aEAIQGg";
	public const string advancedAdsAchiev = "CgkIqe6wvP0aEAIQGw";
    public const string superAdvancedAdsAchiev = "CgkIqe6wvP0aEAIQHA";
    public const string mindBlowingAdsAchiev = "CgkIqe6wvP0aEAIQHQ";
    // Leaderboards
    public static string scoreLeaderboardId = "CgkIqe6wvP0aEAIQAQ"; 
	public static string timeLeaderboardId = "CgkIqe6wvP0aEAIQAg";
	public static string bubblesCountLeaderboardId = "CgkIqe6wvP0aEAIQEQ";

	public static void ReportResult(string leaderboardId, long result){
		Social.ReportScore (result, leaderboardId, (bool success) => {
			if (success){
				Debug.Log(string.Format("Result {0} has been added to leaderboard!", result));
			}
			else{
				Debug.Log("Error updating leaderboard...");
			}
		});
	}

	public static void UnlockAchievement(string achievementId){
		Social.ReportProgress(achievementId, 100.0f, (bool success) => {
			if (success){
				Debug.Log(string.Format("Achievement {0} has been added to ahievements!", achievementId));
			}
			else{
				Debug.Log("Error updating ahievements...");
			}
		});
	}

	public static void GetLeaderboardRank(string leaderboardId){
		int rank = 0;
		int oldRank = PlayerPrefs.GetInt (leaderboardId, 0);
		PlayGamesPlatform.Instance.LoadScores (
			leaderboardId,
			LeaderboardStart.PlayerCentered,
			1,
			LeaderboardCollection.Public,
			LeaderboardTimeSpan.AllTime,
			(LeaderboardScoreData data) => {
				rank = data.PlayerScore.rank;
				switch (rank){
					case 1:
						GooglePlayActions.UnlockAchievement (GooglePlayActions.doingSomethingAchiev);
						break;
					case 2:
						GooglePlayActions.UnlockAchievement (GooglePlayActions.bitNotEnoughAchiev);
						break;
					case 3:
						GooglePlayActions.UnlockAchievement (GooglePlayActions.prettyGoodAchiev);
						break;
					}
				if (oldRank - rank >= 99) {
					GooglePlayActions.UnlockAchievement (GooglePlayActions.climberAchiev);
				}
				PlayerPrefs.SetInt(leaderboardId, rank);
			});
	}
}
