#import "UnityiOSPlugin.h"

#pragma mark Unity native plugin
#ifdef __cplusplus
extern "C" {
#endif
    void TotalPoins(const char* points){
        NSString *messageFromUnity = [NSString stringWithUTF8String:points];
        NSLog(@"%@",messageFromUnity);
    }
#ifdef __cplusplus
}
#endif
