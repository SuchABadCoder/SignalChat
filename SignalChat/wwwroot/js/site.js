var createRoomBtn = document.getElementById('create-room-btn')
var createRoomModal = document.getElementById('create-room-modal')
var editMessageModal = document.getElementById("edit-message-modal")

createRoomBtn.addEventListener('click', function () {
    createRoomModal.classList.add('active')
})


var closeModal = function() {
    createRoomModal.classList.remove('active')
}

function CloseEditModal() {
    editMessageModal.classList.remove('active')
}