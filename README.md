# VIVEEyeTrackerCSV

此项目通过OpenXR SDK获取HTC VIVE设备的眼动追踪数据，并保存为CSV格式文件。支持在Unity编辑器中运行和打包成PC/Android应用。

## 主要文件说明

### 📁 EyeTrackerDataCSV
包含眼动数据记录脚本：
- **`EyeTrackerDataWrite.cs`**  
  核心数据采集脚本，功能包括：
  - 通过OpenXR API获取左右眼注视点位置
  - 记录瞳孔直径、眨眼状态等生物特征数据
  - 按时间戳保存数据到CSV文件
  - 自动在项目根目录生成`EyeData_YYYYMMDD_HHMMSS.csv`文件

### 🎮 测试场景
- **`EyeDataWrite.unity`**  
  预制测试场景，包含：
  - 眼动数据采集控制器
  - 实时数据显示UI面板
  - 退出、修改Tracking Origin

## 使用指南

### 基本操作
1. 导入VIVE OpenXR SDK（详见下方注意事项）
2. 打开 `EyeDataWrite.unity` 场景
3. 如在编辑器内，点击Play按钮启动眼动追踪。
4. 采集自动开始
5. 数据将自动保存至对应的路径的CSV文件

#### PC模式下存储路径：
    C:\Users\[Username]\AppData\LocalLow\DefaultCompany\TestOpenXR251

#### Android模式下存储路径：
    /sdcard/Download

### 打包说明
支持两种平台打包：
```bash
# 打包Windows应用
File > Build Settings > 选择PC平台 > Build

# 打包VIVE一体机APK
File > Build Settings > 切换Android平台 
Player Settings > 设置Package Name > Build
