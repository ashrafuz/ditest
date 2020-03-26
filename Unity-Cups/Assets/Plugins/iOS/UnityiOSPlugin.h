#ifdef UNITY_4_2_0
#import "UnityAppController.h"
#else
#import "AppController.h"
#endif

// Native code plugin
#pragma mark Unity native plugin

#ifdef __cplusplus
extern "C" {
#endif
void TotalPoins(const char* points);
 
#ifdef __cplusplus
}
#endif

