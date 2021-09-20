import { smoothScrollTo, hookFileUploadEvent } from './common';
import homepage from './homepage';
import activitiespage from './activityInfo';

export const SmoothScrollTo = (element) => smoothScrollTo(element);
export const HookFileUploadEvent = (previewImg, fileUploadRefId, discardBtn, imgContainerId, originalSrc) => hookFileUploadEvent(previewImg, fileUploadRefId, discardBtn, imgContainerId, originalSrc);
export const PlayVideo = (src) => homepage.playVideo(src);
export const FilterCarousel = () => homepage.filterCarousel();
export const LogoBanerCarousel = () => homepage.logoBanerCarousel();
export const SetUserLocation = () => homepage.setUserLocation();
export const GetUserLocation = () => homepage.getUserLocation();
export const IncreaseNumber = () => homepage.increaseNumber();
export const OtherAct = () => activitiespage.otherAct();
export const RankCarousel = () => homepage.rankCarousel();