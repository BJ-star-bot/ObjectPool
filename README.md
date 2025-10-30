# ObjectPool
# Unity Object Pool & Grid Partition System

一个用于高性能测试的对象池与分区更新系统。实现了：
- 泛型对象池管理（ObjectPool）
- 分区空间划分（GridPartition）
- 自定义对象更新（ICustomUpdateObject）
- 实时帧率与P95性能监控（FrameTime）

## 🧱 模块结构
ObjectPool.cs       —— 对象池系统  
GridPartition.cs     —— 空间分区系统  
AddObjectManager.cs  —— 生成与调度逻辑  
TestBall.cs          —— 测试物体逻辑  
FrameTime.cs         —— FPS 统计UI  
ShowFrameUI.cs       —— 面板开关控制  

## 🚀 特性
- 分帧更新 + 局部更新双优化
- 100k+ 对象稳定运行
- 可实时切换对象池模式
- 自带帧率、活跃数监控面板

## ⚙️ 使用方式
1. 在场景中放置 `AddObjectManager`
2. 绑定 `TestBall` prefab
3. 按下 `F` 开始生成
4. 按 `Tab` 打开性能UI
