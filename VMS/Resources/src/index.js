import { smoothScrollTo, hookFileUploadEvent, floatBackToTop } from './common';
import homepage from './homepage';
import activitiespage from './activityInfo';
import userProfile from './userProfile';
import organizationProfile from './organizationProfile';
import volunteerListPage from './volunteerListPage';
import admin from './admin';
import gloryAvenue from './gloryAvenue';

export const SmoothScrollTo = (element) => smoothScrollTo(element);
export const FloatBackToTop = (isFloat) => floatBackToTop(isFloat);
export const HookFileUploadEvent = (previewImg, fileUploadRefId, discardBtn, imgContainerId, originalSrc) => hookFileUploadEvent(previewImg, fileUploadRefId, discardBtn, imgContainerId, originalSrc);
export const PlayVideo = (src) => homepage.playVideo(src);
export const FilterCarousel = () => homepage.filterCarousel();
export const LogoBanerCarousel = () => homepage.logoBanerCarousel();
export const SetUserLocation = () => homepage.setUserLocation();
export const GetUserLocation = () => homepage.getUserLocation();
export const IncreaseNumber = () => homepage.increaseNumber();
export const OtherAct = (count) => activitiespage.otherAct(count);
export const ProfileCarousel = () => userProfile.profileCarousel();
export const ActCarousel = () => userProfile.actCarousel();
export const ShowResult = () => homepage.showResult();
export const InformationCarousel = () => organizationProfile.informationCarousel();
export const OrganizeCarousel = () => organizationProfile.organizeCarousel();
export const RankCarousel = () => homepage.rankCarousel();
export const EditProfileCarousel = (count) => organizationProfile.editProfileCarousel(count);
export const AddOutsideClickMenuHandler = (dotnetHelper, methodName) => organizationProfile.addOutsideClickMenuHandler(dotnetHelper, methodName);
export const SaveAs = (filename, bytesBase64) => volunteerListPage.saveAsFile(filename, bytesBase64);

export const SaveAsFile = (filename, bytesBase64) => admin.saveAsFile(filename, bytesBase64);
export const RankingCarousel = () => gloryAvenue.rankingCarousel();

