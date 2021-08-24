using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    /// <summary>
    /// �L�����N�^�[�X�e�[�^�X
    /// </summary>
    public class Status
    {
        /// <summary>
        /// ���x��
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// �q�b�g�|�C���g
        /// </summary>
        public int HitPoint { get; set; }

        /// <summary>
        /// �}�W�b�N�|�C���g
        /// </summary>
        public int MagicPoint { get; set; }

        /// <summary>
        /// �U����
        /// </summary>
        public int Attack { get; set; }

        /// <summary>
        /// �h���
        /// </summary>
        public int Defense { get; set; }

        /// <summary>
        /// �m��
        /// </summary>
        public int Intelligence { get; set; }

        /// <summary>
        /// ���@��R��
        /// </summary>
        public int Resist { get; set; }

        /// <summary>
        /// �f����
        /// </summary>
        public int Agility { get; set; }
    }

    /// <summary>
    /// ��Ԃ̒�`
    /// </summary>
    public enum Conditions : int
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        
        /// <summary>
        /// ��
        /// </summary>
        Poison = 1 << 0
    }

    /// <summary>
    /// �L�����N�^�[��
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ��{�̃X�e�[�^�X
    /// </summary>
    public Status BaseStatus { get; set; }

    /// <summary>
    /// ���݂̃X�e�[�^�X
    /// </summary>
    public Status CurrentStatus { get; set; }

    /// <summary>
    /// ���
    /// </summary>
    public int Condition { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Sprite Face { get; set; }
}
