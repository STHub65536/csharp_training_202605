const modal = document.getElementById("my-modal");
const modalCancelBtn = document.getElementsByClassName("modal-cancel-btn");

function deleteData(ele){
    const buttonId = document.getElementById(ele.id).id;
    const targetId = buttonId.split("-")[1];
    const hiddenInput = document.getElementById("hidden-input-for-sending-id");
    hiddenInput.value = targetId;
    modal.style.display = "block";
}

const btns = Array.from(modalCancelBtn);
btns.forEach(function(btn){
    btn.addEventListener("click", function(){
        modal.style.display = "none";
    });
});

/* 
bfcache(戻る/進むボタン用のキャッシュ)対策.
ページを開いた時に強制リロードする.
*/
window.addEventListener("pageshow", function(event){
  if (event.persisted) {
    window.location.reload();
  }
});