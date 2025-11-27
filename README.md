# ObjectPool

Unity 版本的高性能对象池、空间分区与帧率监控示例项目。通过组合泛型对象池、定制更新驱动与 UI 面板，演示了如何在场景中稳定驱动数以万计的物体。

## 功能概览

| 模块 | 内容概述 |
| --- | --- |
| 对象池（ObjectPool.cs / AddObjectManager.cs / TestBall.cs） | 泛型池自动扩容、复用实例，并在 UI 中显示 `totalCount`、`activeCount` 等统计，便于压测与调参。 |
| 空间分区（GridPartition.cs / BallTransform.cs） | 基于 `Vector2Int` 的网格管理，支持 O(1) 迁移、近邻检索与 `GetNearSnapshot` 局部更新。 |
| 自定义更新（CustomUpdateManager / UpdateInvoker.cs） | 通过固定步长广播 `customUpdate`，让对象在启用/禁用时动态订阅，避免受 Unity `Update` 频率限制。 |
| UGUI 监控（FrameTime.cs / ShowFrameuI.cs） | 统计平均 FPS、最大帧耗、P95 与对象数，并用 Tab 键滑出/隐藏调试面板。 |
| LoadScene | 提供统一的场景加载流程示例，可在压测时快速切换或增量加载不同测试地形。 |
| LeetCode | 简要记录日常刷题进度，表明持续在 LeetCode 上练习数据结构与算法以反哺项目优化。 |

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

通过该示例可以快速验证对象池与空间分区策略在 Unity 下的运行表现，并作为扩展到正式项目的基础。欢迎根据自身需求调整生成逻辑、分区规则或 UI 呈现。
