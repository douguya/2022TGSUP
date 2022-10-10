using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class I_Day_Animation : MonoBehaviour
{

    private VideoClip Day_video; //�Đ�����r�f�I���i�[
    //public video[] video_folder = new video[31]; //���t�ōĐ�����r�f�I���i�[
    public videos[] Month = new videos[12];

    [System.Serializable]
    public class videos//week�̎q�E����̃I�u�W�F�N�g�̎擾
    {
        public VideoClip[] Day = new VideoClip[31];
    }

    //���t���󂯂Ƃ�Ή�����r�f�I��n��
    public VideoClip play_video(string Day)
    {
        video_select(Day); //�Ή�����r�f�I��T��
        return Day_video; //�r�f�I��n��
    }

    //���t����Ή�����r�f�I��T��
    private void video_select(string Day)
    {
        char[] Char_day = Day.ToCharArray(); //���t��char�^�ɕϊ�
        int video_month = Toint(Char_day[0]); //���t�̌����擾
        int video_day = Toint(Char_day[2]); //���t�̓������擾
        Day_video = Month[video_month - 1].Day[video_day - 1]; //���t�ɑΉ�����r�f�I���i�[����
    }

    //char�^��int�^�ɕϊ�����
    private int Toint(char self)
    {
        return self - '0';
    }
}


