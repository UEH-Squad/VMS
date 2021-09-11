var vms;(()=>{"use strict";var e,t,r,o,n={},i={};function a(e){var t=i[e];if(void 0!==t)return t.exports;var r=i[e]={exports:{}};return n[e].call(r.exports,r,r.exports,a),r.exports}a.m=n,t=Object.getPrototypeOf?e=>Object.getPrototypeOf(e):e=>e.__proto__,a.t=function(r,o){if(1&o&&(r=this(r)),8&o)return r;if("object"==typeof r&&r){if(4&o&&r.__esModule)return r;if(16&o&&"function"==typeof r.then)return r}var n=Object.create(null);a.r(n);var i={};e=e||[null,t({}),t([]),t(t)];for(var l=2&o&&r;"object"==typeof l&&!~e.indexOf(l);l=t(l))Object.getOwnPropertyNames(l).forEach((e=>i[e]=()=>r[e]));return i.default=()=>r,a.d(n,i),n},a.d=(e,t)=>{for(var r in t)a.o(t,r)&&!a.o(e,r)&&Object.defineProperty(e,r,{enumerable:!0,get:t[r]})},a.f={},a.e=e=>Promise.all(Object.keys(a.f).reduce(((t,r)=>(a.f[r](e,t),t)),[])),a.u=e=>e+".vms.js",a.g=function(){if("object"==typeof globalThis)return globalThis;try{return this||new Function("return this")()}catch(e){if("object"==typeof window)return window}}(),a.o=(e,t)=>Object.prototype.hasOwnProperty.call(e,t),r={},o="vms:",a.l=(e,t,n,i)=>{if(r[e])r[e].push(t);else{var l,s;if(void 0!==n)for(var c=document.getElementsByTagName("script"),u=0;u<c.length;u++){var d=c[u];if(d.getAttribute("src")==e||d.getAttribute("data-webpack")==o+n){l=d;break}}l||(s=!0,(l=document.createElement("script")).charset="utf-8",l.timeout=120,a.nc&&l.setAttribute("nonce",a.nc),l.setAttribute("data-webpack",o+n),l.src=e),r[e]=[t];var p=(t,o)=>{l.onerror=l.onload=null,clearTimeout(g);var n=r[e];if(delete r[e],l.parentNode&&l.parentNode.removeChild(l),n&&n.forEach((e=>e(o))),t)return t(o)},g=setTimeout(p.bind(null,void 0,{type:"timeout",target:l}),12e4);l.onerror=p.bind(null,l.onerror),l.onload=p.bind(null,l.onload),s&&document.head.appendChild(l)}},a.r=e=>{"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})},(()=>{var e;a.g.importScripts&&(e=a.g.location+"");var t=a.g.document;if(!e&&t&&(t.currentScript&&(e=t.currentScript.src),!e)){var r=t.getElementsByTagName("script");r.length&&(e=r[r.length-1].src)}if(!e)throw new Error("Automatic publicPath is not supported in this browser");e=e.replace(/#.*$/,"").replace(/\?.*$/,"").replace(/\/[^\/]+$/,"/"),a.p=e})(),(()=>{var e={179:0};a.f.j=(t,r)=>{var o=a.o(e,t)?e[t]:void 0;if(0!==o)if(o)r.push(o[2]);else{var n=new Promise(((r,n)=>o=e[t]=[r,n]));r.push(o[2]=n);var i=a.p+a.u(t),l=new Error;a.l(i,(r=>{if(a.o(e,t)&&(0!==(o=e[t])&&(e[t]=void 0),o)){var n=r&&("load"===r.type?"missing":r.type),i=r&&r.target&&r.target.src;l.message="Loading chunk "+t+" failed.\n("+n+": "+i+")",l.name="ChunkLoadError",l.type=n,l.request=i,o[1](l)}}),"chunk-"+t,t)}};var t=(t,r)=>{var o,n,[i,l,s]=r,c=0;for(o in l)a.o(l,o)&&(a.m[o]=l[o]);for(s&&s(a),t&&t(r);c<i.length;c++)n=i[c],a.o(e,n)&&e[n]&&e[n][0](),e[i[c]]=0},r=self.webpackChunkvms=self.webpackChunkvms||[];r.forEach(t.bind(null,0)),r.push=t.bind(null,r.push.bind(r))})();var l={};a.r(l),a.d(l,{FilterCarousel:()=>h,GetUserLocation:()=>T,HookFileUploadEvent:()=>f,IncreaseNumber:()=>L,LogoBanerCarousel:()=>b,OtherAct:()=>w,PlayVideo:()=>v,SetUserLocation:()=>y,SmoothScrollTo:()=>m});const s=e=>t=>{e.src=URL.createObjectURL(t.target.files[0]),e.onload=()=>{URL.revokeObjectURL(e.src)}},c=(e,t)=>()=>{e.src=t},u=e=>{clearTimeout(e.countUpTimeout),e._countUpOrigInnerHTML&&(e.innerHTML=e._countUpOrigInnerHTML,e._countUpOrigInnerHTML=void 0),e.style.visibility=""},d=(e,t={})=>{const{duration:r=1e3,delay:o=16}=t,n=r/o,i=e.toString().split(/(<[^>]+>|[0-9.][,.0-9]*[0-9]*)/),a=[];for(let e=0;e<n;e++)a.push("");for(let e=0;e<i.length;e++)if(/([0-9.][,.0-9]*[0-9]*)/.test(i[e])&&!/<[^>]+>/.test(i[e])){let t=i[e];const r=[...t.matchAll(/[.,]/g)].map((e=>({char:e[0],i:t.length-e.index-1}))).sort(((e,t)=>e.i-t.i));t=t.replace(/[.,]/g,"");let o=a.length-1;for(let e=n;e>=1;e--){let i=parseInt(t/n*e,10);i=r.reduce(((e,{char:t,i:r})=>e.length<=r?e:e.slice(0,-r)+t+e.slice(-r)),i.toString()),a[o--]+=i}}else for(let t=0;t<n;t++)a[t]+=i[e];return a[a.length]=e.toString(),a},p=e=>{var t={Latitude:e.coords.latitude,Longitude:e.coords.longitude};localStorage.setItem("UserLocation",JSON.stringify(t))},g=()=>{const e=e=>{new Waypoint({element:e,handler:function(){((e,t={})=>{const{action:r="start",duration:o=1e3,delay:n=16}=t;if("stop"===r)return void u(e);if(u(e),!/[0-9]/.test(e.innerHTML))return;const i=d(e.innerHTML,{duration:o||e.getAttribute("data-duration"),delay:n||e.getAttribute("data-delay")});e._countUpOrigInnerHTML=e.innerHTML,e.innerHTML=i[0]||"&nbsp;",e.style.visibility="visible";const a=function(){e.innerHTML=i.shift()||"&nbsp;",i.length?(clearTimeout(e.countUpTimeout),e.countUpTimeout=setTimeout(a,n)):e._countUpOrigInnerHTML=void 0};e.countUpTimeout=setTimeout(a,n)})(e),this.destroy()},offset:"bottom-in-view"})};$(document).ready((()=>{const t=document.querySelectorAll(".counter");[].forEach.call(t,e)}))},m=e=>(e=>{$("html, body").stop().animate({scrollTop:$(e).offset().top},500,"swing")})(e),f=(e,t,r,o,n)=>(async(e,t,r,o,n)=>{document.getElementById(o)&&r&&e&&(r.removeEventListener("click",c(e,n)),r.addEventListener("click",c(e,n)));const i=document.getElementById(t);i&&e&&(i.removeEventListener("change",s(e)),i.addEventListener("change",s(e))),(await a.e(216).then(a.t.bind(a,216,23))).init()})(e,t,r,o,n),v=e=>(e=>{const t=document.querySelector(".video-header__source");t.src=e,t.play()})(e),h=()=>{$(".filter__carousel").owlCarousel({loop:!0,margin:0,nav:!0,responsive:{0:{items:4,slideBy:4},1200:{items:6,slideBy:6}}})},b=()=>{$(".logoBaner__carousel").owlCarousel({loop:!0,margin:0,nav:!0,responsive:{0:{items:1}}})},y=()=>(()=>{if(!navigator.geolocation)return null;navigator.geolocation.getCurrentPosition(p)})(),T=()=>(()=>{const e=localStorage.getItem("UserLocation");return e?JSON.parse(e):null})(),L=()=>g(),w=()=>{$(".owl-carousel").owlCarousel({loop:!0,margin:10,nav:!0,navText:['<span class="material-icons">navigate_before</span>','<span class="material-icons">navigate_next</span>'],dots:!1,responsive:{0:{items:3,slideBy:3},1200:{items:4,slideBy:3}}})};vms=l})();