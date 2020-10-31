
export function FilterforBlockedUsers(sourceUserId, thisUserData, userBlocks) {
    // Checks if reader has been blocked
    if (thisUserData.userLevelId === 2) {
        if (userBlocks.some(userBlock => userBlock.sourceUserId === sourceUserId && userBlock.blockedUserId === thisUserData.id)) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}

export function UserBlockMgr(blockedUserId, sourceUserId, userBlocks) {
    // Checks if another user has already been blocked from active user
    return (userBlocks.some(userBlock => userBlock.sourceUserId === sourceUserId && userBlock.blockedUserId === blockedUserId)) ?
        true : false;

}