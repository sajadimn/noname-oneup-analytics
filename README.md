Analytics Unity Package
===

## Description
This package contains the analytics manager for Firebase, GameAnalytics and AppMetrica. Keys stay in your project, never in this package.

## Dependencies
Add this to `Packages/manifest.json` before this package, or it cannot compile:
- [noname-oneup-singleton](https://github.com/sajadimn/noname-oneup-singleton)

## How to use
1. Add this package. It creates the three configs in `Assets/Settings/Analytics/Resources`.
2. Firebase (App and Analytics), GameAnalytics, AppMetrica and the External Dependency Manager install automatically. If that fails, for example with no internet, run `OneUp/Analytics/Install SDKs`.
3. To change SDK versions, edit `Assets/Settings/Analytics/AnalyticsSdkVersionsConfig.asset`, then run `OneUp/Analytics/Update SDKs`. Enter the AppMetrica API key in `AppMetricaAnalyticsConfig` and your GameAnalytics keys in its Settings window. Add Firebase's `google-services.json` and `GoogleService-Info.plist` to your project.
4. Use `AnalyticsManager.Instance`. No prefab or scene object is needed.

Other Firebase packages can listen to `AnalyticsManager.OnFirebaseInitialized`. Push notifications are in [noname-oneup-firebase-notification](https://github.com/sajadimn/noname-oneup-firebase-notification), which does not need this package.
