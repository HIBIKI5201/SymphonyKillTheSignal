using UnityEditor;
using UnityEngine;
using System.IO;

public class ReadmeCreator : Editor
{
    [MenuItem("Tools/Create README.md")]
    public static void CreateReadme()
    {
        // README�t�@�C���̃p�X���w��
        string path = Application.dataPath + "/README.md";

        // README.md �����łɑ��݂��邩���`�F�b�N
        if (File.Exists(path))
        {
            if (!EditorUtility.DisplayDialog("README.md already exists",
                "README.md �t�@�C���͊��ɑ��݂��܂��B�㏑�����܂����H", "�͂�", "������"))
            {
                return;
            }
        }

        // README.md �̓��e���쐬
        string content = "# �v���W�F�N�g��\n\n" +
                         "## �T�v\n" +
                         "���̃v���W�F�N�g�́Z�Z�̂��߂�Unity�v���W�F�N�g�ł��B\n\n" +
                         "## �g�p���@\n" +
                         "1. Unity���J���ăv���W�F�N�g�����[�h���܂��B\n" +
                         "2. `Assets` �t�H���_�ɂ��� `MainScene` ���_�u���N���b�N���ăV�[�����J���܂��B\n" +
                         "3. �v���C�{�^�����N���b�N���ăQ�[�����J�n���܂��B\n\n" +
                         "## �J���Ҍ������\n" +
                         "- `Scripts` �t�H���_�ɂ́A���ׂĂ�C#�X�N���v�g���i�[����Ă��܂��B\n" +
                         "- `Prefabs` �t�H���_�ɂ́A�g�p����邷�ׂẴv���n�u���܂܂�Ă��܂��B\n\n" +
                         "## ���ӓ_\n" +
                         "- ���̃v���W�F�N�g�� Unity 2021.3.0f1 �œ���m�F���s���Ă��܂��B\n" +
                         "- �v���W�F�N�g�̃o�[�W�������A�b�v�O���[�h����ۂ́A�K���o�b�N�A�b�v������Ă��������B\n";

        // README.md �t�@�C�����쐬
        File.WriteAllText(path, content);

        // �A�Z�b�g�f�[�^�x�[�X���X�V���āA�t�@�C����Unity�G�f�B�^�ŕ\���ł���悤�ɂ���
        AssetDatabase.Refresh();

        // �t�@�C�����쐬���ꂽ���Ƃ�ʒm
        Debug.Log("README.md �t�@�C�����쐬����܂���: " + path);
    }
}