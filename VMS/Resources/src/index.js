﻿import lodash from './common/lodash';
import homepage from './homepage';

export const CloneDeep = (object) => lodash.CloneDeep(object);
export const PlayVideo = (src) => homepage.playVideo(src);
export const FilterCarousel = () => homepage.filterCarousel();
export const LogoBanerCarousel = () => homepage.logoBanerCarousel();
export const SetUserLocation = () => homepage.setUserLocation();
export const GetUserLocation = () => homepage.getUserLocation();
export const ShowResult = () => homepage.showResult();