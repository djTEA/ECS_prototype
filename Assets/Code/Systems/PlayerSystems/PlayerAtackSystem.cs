﻿using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.Scripting;


namespace MSuhininTestovoe.B2B
{
    public class PlayerAtackSystem : EcsUguiCallbackSystem, IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsFilter _enemyFilter;
        private EcsPool<IsPlayerCanAttackComponent> _isCanAttackComponentPool;
        private EcsPool<HealthViewComponent> _enemyHealthViewComponentPool;
        private EcsPool<EnemyHealthComponent> _enemyHealthComponentPool;
        private int _entity;


        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = systems.GetWorld().Filter<IsPlayerCanAttackComponent>()
                .End();
            _enemyFilter = systems.GetWorld().Filter<IsEnemyComponent>()
                .Inc<EnemyHealthComponent>()
                .Inc<HealthViewComponent>()
                .End();
            _enemyHealthViewComponentPool = world.GetPool<HealthViewComponent>();
            _enemyHealthComponentPool = world.GetPool<EnemyHealthComponent>();
            _isCanAttackComponentPool = world.GetPool<IsPlayerCanAttackComponent>();
        
         
        }

        [Preserve]
        [EcsUguiClickEvent(UIConstants.BTN_ATACK, WorldsNamesConstants.EVENTS)]
        void OnClickAttack(in EcsUguiClickEvent e)
        {
            foreach (var entity in _filter)
                {
                    foreach (var enemyEntity in _enemyFilter)
                    {
                        ref HealthViewComponent healthView = ref _enemyHealthViewComponentPool.Get(enemyEntity);
                        ref EnemyHealthComponent healthValue = ref _enemyHealthComponentPool.Get(enemyEntity);
                        var currentHealh = healthValue.HealthValue;
                        healthView.Value.size = new Vector2(currentHealh, 1);
                        _entity = enemyEntity;
                    }
                    Attack();
            }
        }
        
        private void Attack()
        {
            ref EnemyHealthComponent healthValue = ref _enemyHealthComponentPool.Get(_entity);
            healthValue.HealthValue -= 1;
            Debug.Log(healthValue.HealthValue);
        }
    }
}