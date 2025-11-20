# ObjectPool

Unity 版本的高性能对象池、空间分区与帧率监控示例项目。通过组合泛型对象池、定制更新驱动与 UI 面板，演示了如何在场景中稳定驱动数以万计的物体。

## 功能概览

- **ObjectPool.cs / IPoolAble**  
  采用泛型实现的轻量对象池，自动扩容、复用实例并跟踪 `totalCount`、`activeCount` 与正在运行的实例列表。
- **GridPartition.cs / IGridObject**  
  基于二维网格的空间分区结构，可用于近邻检索与 O(1) 的物体迁移、移除；脚本提供 `GetNear` 迭代器与 `GetNearSnapshot` 拷贝接口，方便在多线程或自定义调度中使用。
- **CustomUpdateManager & UpdateInvoker**  
  通过全局事件 `customUpdate` 将逻辑更新和 Unity 的 `Update` 解耦，`UpdateInvoker` 以固定步长触发，从而实现“分帧驱动”。
- **TestBall.cs**  
  示例子弹 / 小球，演示如何实现 `IPoolAble`。包含寿命控制、球形射线检测、弹性反弹、命中 `IDamageable` 时造成伤害等逻辑，并在启用/禁用时自动注册到 `customUpdate`。
- **AddObjectManager.cs**  
  负责按照 `objectNumber` 批量生成对象，可在 UI Toggle (`poolTick`) 上切换“实例化”与“从池中取出”两种模式，同时维护统计数据供 UI 使用。
- **FrameTime.cs & ShowFrameUI.cs**  
  `FrameTime` 会每 `windowSeconds` 秒统计平均 FPS、最大帧耗时、P95 值以及对象数量，结果显示在 TMP 文本上；`ShowFrameuI` 允许通过 Tab 键滑动显示/隐藏信息面板。  
- **BallTransform.cs**  
  额外的物理示例脚本，提供简单的重力与反弹计算，可接入 `CustomUpdateManager` 或直接在 `Update` 中调用其 `CustomUpdate`。

## 场景配置步骤

1. **准备 Prefab**  
   - 为测试对象挂载 `TestBall`（或你自己的 `IPoolAble` 实现），并根据需求配置碰撞半径、重力、阻尼等参数。
2. **创建对象池入口**  
   - 在场景中新建空物体并添加 `AddObjectManager`。  
   - 赋值 `ball` Prefab、`poolTick` Toggle（控制是否启用对象池）、`SFI`（性能面板引用）以及生成速率 `objectNumber`、最大并发 `maxObjectNumber`。
3. **自定义更新驱动**  
   - 在任意物体上添加 `UpdateInvoker`，设置 `updateTime`（例如 0.02f），为所有订阅者提供固定频率的 `customUpdate` 回调。
4. **性能监控 UI**  
   - 创建用于显示数据的 TMP 文本与 Toggle，并将其拖拽到 `FrameTime` 组件的对应字段。  
   - 将 `AddObjectManager` 引用赋值给 `FrameTime.addObjectManager`，以便实时显示对象数量。  
   - 将信息面板 / 选项面板 RectTransform 绑定到 `ShowFrameuI`，即可通过 Tab 键控制显隐。

## 运行与操作

- `F` 键：开始 / 暂停批量生成对象（`AddObjectManager.start`）。  
- `Tab` 键：切换帧率与调试面板的显示状态。  
- UI Toggle (`poolTick`)：开启时从对象池中借出实例，关闭时直接 `Instantiate`，方便对比两种模式。  
- 面板 Toggle (`addObjectTick`)：可通过 UI 控制 `AddObjectManager` 是否在运行。

## 扩展与自定义

- **扩展 IPoolAble**：实现 `SetPool`、`OnSpawn`、`OnDespawn` 并在生成时调用 `pool.GetObject()`，即可将任意 MonoBehaviour 接入对象池。
- **接入 GridPartition**：让对象实现 `IGridObject` 并通过 `GridPartition.Add/Remove/UpdateCell` 维护位置，随后即可用 `GetNear` 或 `GetNearSnapshot` 限定范围查询。
- **命中反馈**：`TestBall` 在命中 Collider 时会查找 `IDamageable` 并传入 `DamageContext`，可根据项目中的伤害系统拓展。
- **性能监控**：`FrameTime` 会在 UI 中展示平均帧率、最大帧耗时与 P95 值（毫秒），便于衡量整体与长尾表现；如需更多统计，可在 `deltaTimeList` 的基础上添加指标。

通过该示例可以快速验证对象池与空间分区策略在 Unity 下的运行表现，并作为扩展到正式项目的基础。欢迎根据自身需求调整生成逻辑、分区规则或 UI 呈现。*** End Patch>*** End Patch
